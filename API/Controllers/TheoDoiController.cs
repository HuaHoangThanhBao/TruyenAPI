using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer;
using CoreLibrary.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using CoreLibrary.Models;
using API.Extensions;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheoDoiController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public TheoDoiController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("{key}")]
        public async Task<IActionResult> GetAllTheoDois(string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var theoDois = await _repository.TheoDoi.GetAllTheoDoisAsync();
                var theoDoisResult = _mapper.Map<IEnumerable<TheoDoiDto>>(theoDois);

                return Ok(theoDoisResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllTheoDois" });
            }
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("{id}/{key}", Name = "TheoDoiById")]
        public async Task<IActionResult> GetTheoDoiById(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var theoDoi = await _repository.TheoDoi.GetTheoDoiByIdAsync(id);
                if (theoDoi == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "TheoDoi không tồn tại" });
                }
                else
                {
                    var theoDoiResult = _mapper.Map<TheoDoiDto>(theoDoi);
                    return Ok(theoDoiResult);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTheoDoiById" });
            }
        }

        [HttpPost("{key}")]
        public IActionResult CreateTheoDoi(string key, [FromBody] TheoDoiForCreationDto theoDoi)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (theoDoi == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var theoDoiEntity = _mapper.Map<TheoDoi>(theoDoi);

                var response = _repository.TheoDoi.CreateTheoDoi(theoDoiEntity);
                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                var createdTheoDoi = _mapper.Map<TheoDoiDto>(theoDoiEntity);

                return Ok(createdTheoDoi);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateTheoDoi" });
            }
        }

        [HttpPut("{id}/{key}")]
        public async Task<IActionResult> UpdateTheoDoi(int id, string key, [FromBody] TheoDoiForUpdateDto theoDoi)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (theoDoi == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var theoDoiEntity = await _repository.TheoDoi.GetTheoDoiByIdAsync(id);
                if (theoDoiEntity == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "TheoDoi không tồn tại" });
                }

                _mapper.Map(theoDoi, theoDoiEntity);

                ResponseDetails response = _repository.TheoDoi.UpdateTheoDoi(theoDoiEntity);

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateTheoDoi" });
            }
        }

        [HttpDelete("{id}/{key}")]
        public async Task<IActionResult> DeleteTheoDoi(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var theoDoi = await _repository.TheoDoi.GetTheoDoiByIdAsync(id);
                if (theoDoi == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID TheoDoi không tồn tại" });
                }

                ResponseDetails response = _repository.TheoDoi.DeleteTheoDoi(theoDoi);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteTheoDoi" });
            }
        }
    }
}
