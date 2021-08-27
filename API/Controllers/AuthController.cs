using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Extensions;
using API.Service;
using AutoMapper;
using CoreLibrary;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Helpers;
using CoreLibrary.Models;
using DataAccessLayer;
using EmailService;
using LoggerService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private IRepositoryWrapper _repository;
        private readonly JwtHandler _jwtHandler;
        private readonly IEmailSender _emailSender;
        private readonly ILoggerManager _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthController(UserManager<ApplicationUser> userManager, IRepositoryWrapper repository, IMapper mapper, ILoggerManager logger, JwtHandler jwtHandler, IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager, ITokenService tokenService, IConfiguration config)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _emailSender = emailSender;
            _jwtHandler = jwtHandler;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _config = config;
        }

        [HttpGet("sendmail")]
        public async Task<IActionResult> SendMail([FromQuery] SendMailParameters mail)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

            if (mail.Address == "" || mail.Address == null)
                return BadRequest("Địa chỉ email không được để trống");

            var message = new Message(new string[] { mail.Address }, mail.Subject, mail.Content, null);
            await _emailSender.SendEmailAsync(message);

            return Ok("Email gửi thành công!");
        }

        [HttpPost("registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null || !ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<ApplicationUser>(userForRegistration);

            //Nếu validate có lỗi thì trả về bad request
            var validate = _repository.Authenticate.ValidateRegistration(userForRegistration);
            if(validate != null)
            {
                return BadRequest(validate);
            }

            var userFinding = await _userManager.FindByEmailAsync(userForRegistration.Email);

            if (userFinding != null)
                return BadRequest(new AuthResponseDto { Message = "Email này đã tồn tại. Vui lòng nhập email khác!" });

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            var roleExist = await _roleManager.RoleExistsAsync("User");
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            await _userManager.AddToRoleAsync(user, "User");

            await _userManager.SetTwoFactorEnabledAsync(user, true);

            var response = _repository.User.CreateUser(
            new User()
            {
                Quyen = userForRegistration.Quyen,
                UserName = user.LastName + " " + user.FirstName,
                ApplicationUserID = user.Id
            });

            if (response.StatusCode == ResponseCode.Success)
            {
                _repository.Save();
            }
            else
            {
                _logger.LogError($"Lỗi khi đăng ký user với email {userForRegistration.Email}");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", user.Email }
            };
            /**************/

            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);

            var message = new Message(new string[] { userForRegistration.Email },
                "Xác thực tài khoản", $"Bạn vui lòng nhấn vào đường dẫn này để tiến hành xác thực tài khoản: {callback}", null);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });


            /**************/
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email);

            if (user == null)
                return BadRequest(new AuthResponseDto { Message = "Tài khoản không tồn tại!" });

            //Nếu validate có lỗi thì trả về bad request
            var validate = _repository.Authenticate.ValidateLogin(userForAuthentication);
            if (validate != null)
            {
                return BadRequest(validate);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized(new AuthResponseDto { Message = "Tài khoản này chưa được xác nhận. Vui lòng xác nhận tài khoản trước khi đăng nhập" });

            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
            {
                if(await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogWarn("User với email " + user.Email + " cố gắng đăng nhập sai thông tin");
                    return Unauthorized(new AuthResponseDto
                    {
                        Message = "Vì lý do bảo mật nên tài khoản đã bị khóa vì bạn đăng nhập thất bại quá 3 lần, " +
                        "chúng tôi đã gửi mail cho bạn để reset mật khẩu. Bạn vui lòng kiểm tra. Xin cảm ơn."
                    });
                }

                var accesFailedCount = await _userManager.GetAccessFailedCountAsync(user);

                await _userManager.AccessFailedAsync(user);

                if(accesFailedCount == Data.MaxFailedAccessAttempts - 1)
                {
                    await _userManager.SetLockoutEnabledAsync(user, true);
                }

                if (await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogWarn("User với email " + user.Email + " đã quá hạn đăng nhập 3 lần, đã tạm lock tài khoản");
                    
                    var content = $"Tài khoản của bạn đã bị tạm khóa vì quá hạn đăng nhập sai 3 lần. " +
                        $"Vui lòng bấm vào đường dẫn này để reset mật khẩu: {userForAuthentication.clientURI}";
                    var message = new Message(new string[] { userForAuthentication.Email }, "Locked out account information", content, null);
                    await _emailSender.SendEmailAsync(message);

                    return Unauthorized(new AuthResponseDto { Message = "Vì lý do bảo mật nên tài khoản đã bị khóa nếu đăng nhập thất bại quá 3 lần, " +
                        "chúng tôi đã gửi mail cho bạn để reset mật khẩu" });
                }

                return Unauthorized(new AuthResponseDto { Message = "Thông tin đăng nhập sai" });
            }

            /**************/

            if (await _userManager.GetTwoFactorEnabledAsync(user))
                return await GenerateOTPFor2StepVerification(user);

            //var token = await _jwtHandler.GenerateToken(user);

            await _userManager.ResetAccessFailedCountAsync(user);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, /*Token = token*/ });
        }

        private async Task<IActionResult> GenerateOTPFor2StepVerification(ApplicationUser user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                return Unauthorized(new AuthResponseDto { Message = "Thiếu thông tin provider để token đăng nhập." });
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var message = new Message(new string[] { user.Email }, "Mã xác nhận", 
                $"Mã xác nhận của bạn là: {token}. Vui lòng nhập mã này vào ô xác nhận để hoàn tất tiến trình đăng nhập", null);
            await _emailSender.SendEmailAsync(message);

            return Ok(new AuthResponseDto { Is2StepVerificationRequired = true, Provider = "Email" });
        }

        [HttpPost("LoginVerification")]
        public async Task<IActionResult> LoginVerification([FromBody] TwoFactorDto twoFactorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userApp = await _userManager.FindByEmailAsync(twoFactorDto.Email);
            if (userApp == null)
                return BadRequest(new AuthResponseDto { Message = "Tài khoản không tồn tại!" });

            var validVerification = await _userManager.VerifyTwoFactorTokenAsync(userApp, twoFactorDto.Provider, twoFactorDto.Token);
            if (!validVerification)
                return BadRequest("Token để xác thực đăng nhập không hợp lệ!");

            var user = await _repository.User.GetUserByApplicationUserIDAsync(userApp.Id);

            var claims = await _jwtHandler.GenerateClaims(userApp, user);

            await _userManager.ResetAccessFailedCountAsync(userApp);

            var accessToken = _tokenService.GenerateAccessToken(claims, _config);
            var refreshToken = _tokenService.GenerateRefreshToken();

            ResponseDetails response = _repository.User.UpdateUserRefreshToken(
                user,
                refreshToken,
                DateTime.Now.AddMinutes(Convert.ToDouble(_config[$"{NamePars.JwtSettings}:{NamePars.ExpireTime}"]))
            );
            if (response.StatusCode == ResponseCode.Success)
            {
                _repository.Save();
            }
            else
            {
                _logger.LogError($"Lỗi khi cấp refresh token khi xác thực đăng nhập cho user với id {user.UserID}");
            }

            return Ok(new
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpGet("RegistrationVerification")]
        public async Task<IActionResult> RegistrationVerification([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Tài khoản không tồn tại!");

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest("Mã token để xác thực đăng ký không hợp lệ!");

            return Ok();
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

            if (!ModelState.IsValid)
                return BadRequest("Các trường dữ liệu nhập vào chưa chính xác!");

            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest("Tài khoản không tồn tại!");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", forgotPasswordDto.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);

            var message = new Message(new string[] { forgotPasswordDto.Email }, "Đặt lại mật khẩu", $"Vui lòng nhấn vào đường dẫn này để tiến hành đặt lại mật khẩu: {callback}", null);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

            if (!ModelState.IsValid)
                return BadRequest("Các trường dữ liệu nhập vào chưa chính xác!");

            //Nếu validate có lỗi thì trả về bad request
            var validate = _repository.Authenticate.ValidateResetPassword(resetPasswordDto);
            if (validate != null)
            {
                return BadRequest(validate);
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Tài khoản không tồn tại!");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }

            await _userManager.SetLockoutEnabledAsync(user, false);

            await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

            return Ok();
        }


        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

            if (!ModelState.IsValid)
                return BadRequest("Các trường dữ liệu nhập vào chưa chính xác!");

            //Nếu validate có lỗi thì trả về bad request
            var validate = _repository.Authenticate.ValidateUpdatePassword(updatePasswordDto);
            if (validate != null)
            {
                return BadRequest(validate);
            }

            var user = await _userManager.FindByEmailAsync(updatePasswordDto.Email);
            if (user == null)
                return BadRequest("Tài khoản không tồn tại!");

            if (!await _userManager.CheckPasswordAsync(user, updatePasswordDto.OldPassword))
                return BadRequest(new AuthResponseDto { Message = "Mật khẩu cũ không chính xác" });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", updatePasswordDto.Email }
            };

            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, updatePasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }

            await _userManager.SetLockoutEnabledAsync(user, false);

            await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

            return Ok();
        }
    }
}
