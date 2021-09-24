using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Helpers;
using CoreLibrary.Models;
using DataAccessLayer;
using LoggerService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IRepositoryWrapper repository, IMapper mapper, ILoggerManager logger)
        {
            _userManager = userManager;
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var users = await _repository.User.GetAllUsersAsync();

                var applicationUsers = await _userManager.Users.ToListAsync();
                var result = (from m in users
                              join n in applicationUsers
                              on m.ApplicationUserID equals n.Id
                              select new UserDto { 
                                  UserID = m.UserID,
                                  LockoutEnabled = n.LockoutEnabled, 
                                  Quyen = m.Quyen, 
                                  UserName = m.UserName, 
                                  HinhAnh = m.HinhAnh,
                                  TinhTrang = m.TinhTrang
                              });

                var usersResult = _mapper.Map<IEnumerable<UserDto>>(result);

                return Ok(usersResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllUsers" });
            }
        }

        [HttpGet("{userid}/details")]
        public async Task<IActionResult> GetUserByDetails(string userid)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var user = await _repository.User.GetUserByUserIDDetailAsync(userid);

                if (user == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "User không tồn tại" });
                }
                else
                {
                    //var UserResult = _mapper.Map<UserDto>(User);
                    var userApp = await _userManager.FindByIdAsync(user.ApplicationUserID);
                    return Ok(new UserInfo() 
                    { 
                        Email = userApp.Email, 
                        UserName = user.UserName, 
                        FirstName = userApp.FirstName, 
                        LastName = userApp.LastName, 
                        HinhAnh = user.HinhAnh,
                        TheoDois = user.TheoDois,
                        BinhLuans = user.BinhLuans
                    });
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetUserByDetails" });
            }
        }

        [HttpPost("pheduyetuser")]
        public async Task<IActionResult> PheDuyetUser([FromBody] UserForAcceptanceDto user)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (user == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                if (user.Quyen != Data.EditorRole)
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Bạn không có quyền phê duyệt user này" });

                var userFound = await _repository.User.GetUserByIDAsync(user.UserID.ToString());
                if (userFound == null)
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "User không tồn tại" });

                var userApp = await _userManager.FindByIdAsync(userFound.ApplicationUserID);

                //Phê duyệt rồi thì tắt lockout accountd đi
                await _userManager.SetLockoutEnabledAsync(userApp, false);

                return Ok(new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Phê duyệt thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Lỗi khi create new user: " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm PheDuyetUser" });
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserForCreationDto user)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (user == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var userEntity = _mapper.Map<User>(user);

                var response = _repository.User.CreateUser(userEntity);
                if (response != null)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                var createdUser = _mapper.Map<UserDto>(userEntity);

                _logger.LogInfo("User mới đăng ký với ID là: " + userEntity.UserID);

                return Ok(createdUser);
            }
            catch(Exception ex)
            {
                _logger.LogError("Lỗi khi create new user: " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateUser" });
            }
        }

        [HttpPut("UpdateUserAvatar")]
        public async Task<IActionResult> UpdateAvatar([FromBody] UpdateUserAvatarDto updateUserAvatarDto)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (!ModelState.IsValid)
                    return BadRequest("Các trường dữ liệu nhập vào chưa chính xác!");

                if (updateUserAvatarDto.HinhAnh == "")
                    return BadRequest("Hình ảnh không được để trống!");

                var userRepo = await _repository.User.GetUserByIDAsync(updateUserAvatarDto.UserID);

                if (userRepo == null)
                    return BadRequest("Tài khoản không tồn tại!");

                //Ta cập nhật lại bảng user nhưng chỉ với trường dữ liệu là HinhAnh
                ResponseDetails response = _repository.User.UpdateUser(new User()
                {
                    UserID = userRepo.UserID,
                    ApplicationUserID = userRepo.ApplicationUserID,
                    Quyen = userRepo.Quyen,
                    TinhTrang = userRepo.TinhTrang,
                    HinhAnh = updateUserAvatarDto.HinhAnh,
                    RefreshToken = userRepo.RefreshToken,
                    RefreshTokenExpiryTime = userRepo.RefreshTokenExpiryTime,
                    UserName = userRepo.UserName
                });

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi cập nhật avatar cho user với id {updateUserAvatarDto.UserID}: ${ex}");
            }

            return Ok();
        }

        [HttpPut("{userid}")]
        public async Task<IActionResult> UpdateUser(string userid, [FromBody] UserForUpdateDto user)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (user == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var userEntity = await _repository.User.GetUserByIDAsync(userid);
                if (userEntity == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "User không tồn tại" });
                }

                _mapper.Map(user, userEntity);

                ResponseDetails response = _repository.User.UpdateUser(userEntity);

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError("Lỗi khi update user " + userid + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateUser" });
            }
        }

        [HttpDelete("{userid}")]
        public async Task<IActionResult> DeleteUser(string userid)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var user = await _repository.User.GetUserByIDAsync(userid);
                if (user == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID truyện không tồn tại" });
                }

                ResponseDetails response = _repository.User.DeleteUser(user);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError("Lỗi khi delete user " + userid + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteUser" });
            }
        }
    }
}
