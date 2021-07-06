using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using Microsoft.AspNetCore.Cors;
using CoreLibrary.Helpers;

namespace API.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TheLoaiController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public TheLoaiController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTheLoais()
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var theLoais = await _repository.TheLoai.GetAllTheLoaisAsync();
                var theLoaisResult = _mapper.Map<IEnumerable<TheLoaiDto>>(theLoais);

                return Ok(theLoaisResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllTheLoais" });
            }
        }

        [HttpGet("{id}", Name = "TheLoaiById")]
        public async Task<IActionResult> GetTheLoaiById(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var theLoai = await _repository.TheLoai.GetTheLoaiByIdAsync(id);
                if (theLoai == null)
                {
                    return NotFound();
                }
                else
                {
                    var theLoaiResult = _mapper.Map<TheLoaiDto>(theLoai);
                    return Ok(theLoaiResult);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTheLoaiById" });
            }
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetTheLoaiByDetails(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var theLoai = await _repository.TheLoai.GetTheLoaiByDetailAsync(id);

                if (theLoai == null)
                {
                    return NotFound();
                }
                else
                {
                    var theLoaiResult = _mapper.Map<TheLoaiDto>(theLoai);
                    return Ok(theLoaiResult);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTheLoaiByDetails" });
            }
        }

        [HttpPost]
        public IActionResult CreateTheLoai([FromBody] IEnumerable<TheLoaiForCreationDto> theLoai)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (theLoai == null)
                {
                    return BadRequest("TheLoai object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var theLoaiEntity = _mapper.Map<IEnumerable<TheLoai>>(theLoai);

                ResponseDetails response = _repository.TheLoai.CreateTheLoai(theLoaiEntity);
                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                var createdTheLoai = _mapper.Map<IEnumerable<TheLoaiDto>>(theLoaiEntity);

                return Ok(createdTheLoai);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateTheLoai" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheLoai(int id, [FromBody] TheLoaiForUpdateDto theLoai)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (theLoai == null)
                {
                    return BadRequest("TheLoai object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var theLoaiEntity = await _repository.TheLoai.GetTheLoaiByIdAsync(id);
                if (theLoaiEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(theLoai, theLoaiEntity);

                ResponseDetails response = _repository.TheLoai.UpdateTheLoai(theLoaiEntity);

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateTheLoai" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheLoai(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var theLoai = await _repository.TheLoai.GetTheLoaiByIdAsync(id);
                if (theLoai == null)
                {
                    return NotFound();
                }

                if (_repository.PhuLuc.TheLoaisInPhuLuc(id).Any())
                {
                    return BadRequest(new ResponseDetails()
                    { 
                        StatusCode = 500, 
                        Message = "Không thể xóa TheLoai này. Tồn tại khóa ngoại tới bảng PhuLucs."
                    });
                }

                ResponseDetails response = _repository.TheLoai.DeleteTheLoai(theLoai);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return NoContent();
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = 500, Message = "Lỗi execption ở hàm DeleteTheLoai" });
            }
        }
    }
}
