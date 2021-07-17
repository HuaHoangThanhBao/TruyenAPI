using DataAccessLayer;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using Microsoft.AspNetCore.Cors;
using CoreLibrary.Helpers;
using LoggerService;
using System;

namespace API.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class NoiDungChuongController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private IMapper _mapper;

        public NoiDungChuongController(IRepositoryWrapper repository, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNoiDungChuongs()
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var noiDungChuongs = await _repository.NoiDungChuong.GetAllNoiDungChuongsAsync();
                var noiDungChuongsResult = _mapper.Map<IEnumerable<NoiDungChuongDto>>(noiDungChuongs);

                return Ok(noiDungChuongsResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllNoiDungChuongs" });
            }
        }

        [HttpGet("{id}", Name = "NoiDungChuongById")]
        public async Task<IActionResult> GetNoiDungChuongById(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var noiDungChuong = await _repository.NoiDungChuong.GetNoiDungChuongByIdAsync(id);
                if (noiDungChuong == null)
                {
                    return NotFound();
                }
                else
                {
                    var noiDungChuongResult = _mapper.Map<NoiDungChuongDto>(noiDungChuong);
                    return Ok(noiDungChuongResult);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTheLoaiById" });
            }
        }

        [HttpPost]
        public IActionResult CreateNoiDungChuong([FromBody] IEnumerable<NoiDungChuongForCreationDto> noiDungChuong)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (noiDungChuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var noiDungChuongEntity = _mapper.Map<IEnumerable<NoiDungChuong>>(noiDungChuong);

                var response = _repository.NoiDungChuong.CreateNoiDungChuong(noiDungChuongEntity);
                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                var createdNoiDungChuong = _mapper.Map<IEnumerable<NoiDungChuongDto>>(noiDungChuongEntity);

                return Ok(createdNoiDungChuong);
            }
            catch(Exception ex)
            {
                _logger.LogError("Gặp lỗi khi tạo mới danh sách nội dung chương: " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception xảy ra ở hàm CreateNoiDungChuong" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNoiDungChuong(int id, [FromBody] NoiDungChuongForUpdateDto noiDungChuong)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (noiDungChuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var noiDungChuongEntity = await _repository.NoiDungChuong.GetNoiDungChuongByIdAsync(id);
                if (noiDungChuongEntity == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Nội dung chương không tồn tại" });
                }

                _mapper.Map(noiDungChuong, noiDungChuongEntity);

                ResponseDetails response = _repository.NoiDungChuong.UpdateNoiDungChuong(noiDungChuongEntity);

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError("Gặp lỗi khi cập nhật nội dung chương với ID " + id + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateNoiDungChuong" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNoiDungChuong(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var noiDungChuong = await _repository.NoiDungChuong.GetNoiDungChuongByIdAsync(id);
                if (noiDungChuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID Nội dung chương không tồn tại" });
                }

                ResponseDetails response = _repository.NoiDungChuong.DeleteNoiDungChuong(noiDungChuong);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError("Gặp lỗi khi xóa nội dung chương với ID " + id + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteNoiDungChuong" });
            }
        }
    }
}
