using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer;
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
    public class PhuLucController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private IMapper _mapper;

        public PhuLucController(IRepositoryWrapper repository, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPhuLucs()
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var phuLucs = await _repository.PhuLuc.GetAllPhuLucsAsync();
                var phuLucsResult = _mapper.Map<IEnumerable<PhuLucDto>>(phuLucs);

                return Ok(phuLucsResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllPhuLucs" });
            }
        }

        [HttpGet("{id}", Name = "PhuLucById")]
        public async Task<IActionResult> GetPhuLucByTruyenId(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var phuLuc = await _repository.PhuLuc.GetPhuLucByTruyenIdAsync(id);
                if (phuLuc == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Phụ lục không tồn tại" });
                }
                else
                {
                    //var phuLucResult = _mapper.Map<PhuLucDto>(phuLuc);
                    return Ok(phuLuc);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetPhuLucById" });
            }
        }

        [HttpPost]
        public IActionResult CreatePhuLuc([FromBody] IEnumerable<PhuLucForCreationDto> phuLuc)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (phuLuc == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var phuLucEntity = _mapper.Map<IEnumerable<PhuLuc>>(phuLuc);

                ResponseDetails response = _repository.PhuLuc.CreatePhuLuc(phuLucEntity);
                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                var createdPhuLuc = _mapper.Map<IEnumerable<PhuLucDto>>(phuLucEntity);

                return Ok(createdPhuLuc);
            }
            catch(Exception ex)
            {
                _logger.LogError("Gặp lỗi khi tạo mới danh sách phụ lục: " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreatePhuLuc" });
            }
        }

        [HttpPut]
        public IActionResult UpdatePhuLuc([FromBody] IEnumerable<PhuLucForUpdateDto> phuLuc)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (phuLuc == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var phuLucsResult = _mapper.Map<IEnumerable<PhuLuc>>(phuLuc);

                ResponseDetails response = _repository.PhuLuc.UpdatePhuLuc(phuLucsResult);

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError("Gặp lỗi khi cập nhật danh sách phụ lục của truyện: " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdatePhuLuc" });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhuLuc(int id)
        {
            try
            {
                var phuLuc = await _repository.PhuLuc.GetPhuLucByIdAsync(id);
                if (phuLuc == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID phụ lục không tồn tại" });
                }

                ResponseDetails response = _repository.PhuLuc.DeletePhuLuc(phuLuc);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError("Gặp lỗi khi cập nhật phụ lục với ID " + id + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeletePhuLuc" });
            }
        }
    }
}
