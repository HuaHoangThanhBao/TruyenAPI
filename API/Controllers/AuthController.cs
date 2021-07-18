using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Extensions;
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

        public AuthController(UserManager<ApplicationUser> userManager, IRepositoryWrapper repository, IMapper mapper, ILoggerManager logger, JwtHandler jwtHandler, IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _emailSender = emailSender;
            _jwtHandler = jwtHandler;
            _roleManager = roleManager;
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


            /**************/
            if (userForRegistration.Password != userForRegistration.ConfirmPassword)
                return BadRequest(new AuthResponseDto { ErrorMessage = "Mật khẩu xác nhận không khớp!" });

            if (userForRegistration.Password.Length < Data.PasswordRequiredLength || userForRegistration.Password.Length > Data.PasswordRequiredMaxLength)
                return BadRequest(new AuthResponseDto { ErrorMessage = 
                    $"Mật khẩu phải có độ dài trong khoảng từ {Data.PasswordRequiredLength} - {Data.PasswordRequiredMaxLength} ký tự" });

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasNoneAlpha = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (hasNoneAlpha.IsMatch(userForRegistration.LastName))
                return BadRequest(new AuthResponseDto { ErrorMessage = "Họ không được chứa ký tự đặc biệt" });

            if (hasNoneAlpha.IsMatch(userForRegistration.FirstName))
                return BadRequest(new AuthResponseDto { ErrorMessage = "Tên không được chứa ký tự đặc biệt" });

            if (!hasNumber.IsMatch(userForRegistration.Password))
                return BadRequest(new AuthResponseDto { ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ số (0-9)" });

            if (!hasUpperChar.IsMatch(userForRegistration.Password))
                return BadRequest(new AuthResponseDto { ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự in hoa (A-Z)" });

            if (!hasNoneAlpha.IsMatch(userForRegistration.Password))
                return BadRequest(new AuthResponseDto { ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự đặc biệt" });

            var userFinding = await _userManager.FindByEmailAsync(userForRegistration.Email);

            if (userFinding != null)
                return BadRequest(new AuthResponseDto { ErrorMessage = "Email này đã tồn tại. Vui lòng nhập email khác!" });

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

            try
            {
                var response = _repository.User.CreateUser(
                    new User()
                    {
                        Username = userForRegistration.FirstName + userForRegistration.LastName,
                        FirstName = userForRegistration.FirstName,
                        LastName = userForRegistration.LastName,
                        Email = userForRegistration.Email,
                        Password = userForRegistration.Password
                    }
                );

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Lỗi khi đăng ký user với email {userForRegistration.Email}: ${ex}");
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

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

            foreach (var cookieKey in Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(cookieKey);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        [HttpPost("checklogin")]
        public async Task<IActionResult> CheckLogin()
        {
            var cookies = Request.Cookies;
            var userIDCookie = cookies["UserID"];

            var user = await _repository.User.GetUserByIDAsync(userIDCookie);
            if (user == null)
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Phiên đăng nhập hết hạn!" });
            }

            return Ok(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = user.UserID.ToString().ToUpper() });
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
                return BadRequest(new AuthResponseDto { ErrorMessage = "Tài khoản không tồn tại!" });

            if(userForAuthentication.Password.Length < Data.PasswordRequiredLength)
            {
                return BadRequest(new AuthResponseDto { ErrorMessage = $"Độ dài mật khẩu phải ít nhất {Data.PasswordRequiredLength} ký tự" });
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Tài khoản này chưa được xác nhận. Vui lòng xác nhận tài khoản trước khi đăng nhập" });

            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
            {
                if(await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogWarn("User với email " + user.Email + " cố gắng đăng nhập sai thông tin");
                    return Unauthorized(new AuthResponseDto
                    {
                        ErrorMessage = "Vì lý do bảo mật nên tài khoản đã bị khóa vì bạn đăng nhập thất bại quá 3 lần, " +
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

                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Vì lý do bảo mật nên tài khoản đã bị khóa nếu đăng nhập thất bại quá 3 lần, " +
                        "chúng tôi đã gửi mail cho bạn để reset mật khẩu" });
                }

                return Unauthorized(new AuthResponseDto { ErrorMessage = "Thông tin đăng nhập sai" });
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
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Thiếu thông tin provider để token đăng nhập." });
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

            var user = await _userManager.FindByEmailAsync(twoFactorDto.Email);
            if (user == null)
                return BadRequest(new AuthResponseDto { ErrorMessage = "Tài khoản không tồn tại!" });

            var validVerification = await _userManager.VerifyTwoFactorTokenAsync(user, twoFactorDto.Provider, twoFactorDto.Token);
            if (!validVerification)
                return BadRequest("Token để xác thực đăng nhập không hợp lệ!");

            //var token = await _jwtHandler.GenerateToken(user);


            /**************/
            var claims = await _jwtHandler.GenerateClaims(user);

            await _userManager.ResetAccessFailedCountAsync(user);

            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = true,
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var userGetting = await _repository.User.GetUserByEmailAsync(twoFactorDto.Email);

            CookieOptions option = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = Data.CookieOptionExpireTime,
                IsEssential = true,
            };

            Response.Cookies.Append("UserID", userGetting.UserID.ToString().ToUpper(), option);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            /**************/

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = userGetting.UserID.ToString().ToUpper()/*Không truyền token mà chỉ truyền userID*/ });
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

            if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
                return BadRequest(new AuthResponseDto { ErrorMessage = "Mật khẩu xác nhận không khớp!" });

            if (resetPasswordDto.Password.Length < Data.PasswordRequiredLength || resetPasswordDto.Password.Length > Data.PasswordRequiredMaxLength)
                return BadRequest(new AuthResponseDto { ErrorMessage = 
                    $"Mật khẩu phải có độ dài trong khoảng từ {Data.PasswordRequiredLength} - {Data.PasswordRequiredMaxLength} ký tự" });

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasNoneAlpha = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasNumber.IsMatch(resetPasswordDto.Password))
                return BadRequest(new AuthResponseDto { ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ số (0-9)" });

            if (!hasUpperChar.IsMatch(resetPasswordDto.Password))
                return BadRequest(new AuthResponseDto { ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự in hoa (A-Z)" });

            if (!hasNoneAlpha.IsMatch(resetPasswordDto.Password))
                return BadRequest(new AuthResponseDto { ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự đặc biệt" });

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
    }
}
