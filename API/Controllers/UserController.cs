using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public UserController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _repository.User.GetAllUsersAsync();
                var usersResult = _mapper.Map<IEnumerable<UserDto>>(users);

                return Ok(usersResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllUsers" });
            }
        }

        [HttpGet("{id}", Name = "UserById")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _repository.User.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "User không tồn tại" });
                }
                else
                {
                    var userResult = _mapper.Map<UserDto>(user);
                    return Ok(userResult);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetUserById" });
            }
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetUserByDetails(Guid id)
        {
            try
            {
                var user = await _repository.User.GetUserByDetailAsync(id);

                if (user == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "User không tồn tại" });
                }
                else
                {
                    //var UserResult = _mapper.Map<UserDto>(User);
                    return Ok(user);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTacGiaByDetails" });
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserForCreationDto user)
        {
            try
            {
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
                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                var createdUser = _mapper.Map<UserDto>(userEntity);

                return Ok(createdUser);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateUser" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserForUpdateDto user)
        {
            try
            {
                if (user == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var userEntity = await _repository.User.GetUserByIdAsync(id);
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
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateUser" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var user = await _repository.User.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID truyện không tồn tại" });
                }

                //if (_repository.Account.AccountsByOwner(id).Any())
                //{
                //    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                //}

                ResponseDetails response = _repository.User.DeleteUser(user);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteUser" });
            }
        }
    }
}
