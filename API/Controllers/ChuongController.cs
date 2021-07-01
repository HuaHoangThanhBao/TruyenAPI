using System.Collections.Generic;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChuongController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public ChuongController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetAllChuongs(string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var chuongs = await _repository.Chuong.GetAllChuongsAsync();
                var chuongsResult = _mapper.Map<IEnumerable<ChuongDto>>(chuongs);

                return Ok(chuongsResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllChuongs" });
            }
        }

        [HttpGet("{id}/{key}", Name = "ChuongById")]
        public async Task<IActionResult> GetChuongById(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var chuong = await _repository.Chuong.GetChuongByIdAsync(id);
                if (chuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Chương không tồn tại" });
                }
                else
                {
                    var chuongResult = _mapper.Map<ChuongDto>(chuong);
                    return Ok(chuongResult);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetChuongById" });
            }
        }

        [HttpGet("{id}/{key}/details")]
        public async Task<IActionResult> GetChuongByDetails(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var chuong = await _repository.Chuong.GetChuongByDetailAsync(id);

                if (chuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Chương không tồn tại" });
                }
                else
                {
                    //var ChuongResult = _mapper.Map<ChuongDto>(Chuong);
                    return Ok(chuong);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTacGiaByDetails" });
            }
        }

        [HttpPost("{key}")]
        public IActionResult CreateChuong(string key, [FromBody] IEnumerable<ChuongForCreationDto> chuong)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (chuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var chuongEntity = _mapper.Map<IEnumerable<Chuong>>(chuong);

                var response = _repository.Chuong.CreateChuong(chuongEntity);
                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                var createdChuong = _mapper.Map<IEnumerable<ChuongDto>>(chuongEntity);

                return Ok(createdChuong);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateChuong" });
            }
        }

        [HttpPut("{id}/{key}")]
        public async Task<IActionResult> UpdateChuong(int id, string key, [FromBody] ChuongForUpdateDto chuong)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (chuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var chuongEntity = await _repository.Chuong.GetChuongByIdAsync(id);
                if (chuongEntity == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Truyện không tồn tại" });
                }

                _mapper.Map(chuong, chuongEntity);

                ResponseDetails response = _repository.Chuong.UpdateChuong(chuongEntity);

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateChuong" });
            }
        }

        [HttpDelete("{id}/{key}")]
        public async Task<IActionResult> DeleteChuong(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var chuong = await _repository.Chuong.GetChuongByIdAsync(id);
                if (chuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID truyện không tồn tại" });
                }

                ResponseDetails response = _repository.Chuong.DeleteChuong(chuong);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteChuong" });
            }
        }
    }
}
