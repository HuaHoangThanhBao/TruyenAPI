using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TacGiaController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public TacGiaController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetAllTacGias(string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var tacGias = await _repository.TacGia.GetAllTacGiasAsync();
                var tacGiasResult = _mapper.Map<IEnumerable<TacGiaDto>>(tacGias);

                return Ok(tacGiasResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllTacGias" });
            }
        }

        [HttpGet("{id}/{key}", Name = "TacGiaById")]
        public async Task<IActionResult> GetTacGiaById(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var tacGia = await _repository.TacGia.GetTacGiaByIdAsync(id);
                if (tacGia == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Tác giả không tồn tại" });
                }
                else
                {
                    var tacGiaResult = _mapper.Map<TacGiaDto>(tacGia);
                    return Ok(tacGiaResult);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTacGiaById" });
            }
        }

        [HttpGet("{id}/{keys}/details")]
        public async Task<IActionResult> GetTacGiaByDetails(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var tacGia = await _repository.TacGia.GetTacGiaByDetailAsync(id);

                if (tacGia == null)
                {
                    return NotFound();
                }
                else
                {
                    var ownerResult = _mapper.Map<TacGiaDto>(tacGia);
                    return Ok(ownerResult);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTacGiaByDetails" });
            }
        }

        [HttpPost("{key}")]
        public IActionResult CreateTacGia(string key, [FromBody] IEnumerable<TacGiaForCreationDto> tacGia)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (tacGia == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var tacGiaEntity = _mapper.Map<IEnumerable<TacGia>>(tacGia);

                ResponseDetails response = _repository.TacGia.CreateTacGia(tacGiaEntity);
                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                var createdTacGia = _mapper.Map<IEnumerable<TacGiaDto>>(tacGiaEntity);

                return Ok(createdTacGia);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateTacGia" });
            }
        }

        [HttpPut("{id}/{key}")]
        public async Task<IActionResult> UpdateTacGia(int id, string key, [FromBody]TacGiaForUpdateDto tacGia)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (tacGia == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var tacGiaEntity = await _repository.TacGia.GetTacGiaByIdAsync(id);
                if (tacGiaEntity == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Tác giả không tồn tại" });
                }

                _mapper.Map(tacGia, tacGiaEntity);

                ResponseDetails response = _repository.TacGia.UpdateTacGia(tacGiaEntity);

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateTacGia" });
            }
        }

        [HttpDelete("{id}/{key}")]
        public async Task<IActionResult> DeleteTacGia(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var tacGia = await _repository.TacGia.GetTacGiaByIdAsync(id);
                if (tacGia == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID tác giả không tồn tại" });
                }

                ResponseDetails response = _repository.TacGia.DeleteTacGia(tacGia);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteTacGia" });
            }
        }
    }
}
