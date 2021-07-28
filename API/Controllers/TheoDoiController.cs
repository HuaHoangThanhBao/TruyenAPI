using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer;
using CoreLibrary.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using CoreLibrary.Models;
using API.Extensions;
using Microsoft.AspNetCore.Cors;
using CoreLibrary.Helpers;
using Newtonsoft.Json;
using LoggerService;
using System;

namespace API.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TheoDoiController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private IMapper _mapper;

        public TheoDoiController(IRepositoryWrapper repository, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTheoDois()
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

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

        [HttpGet("{id}", Name = "TheoDoiById")]
        public async Task<IActionResult> GetTheoDoiById(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

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

        [HttpPost]
        public IActionResult CreateTheoDoi([FromBody] TheoDoiForCreationDto theoDoi)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

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
            catch(Exception ex)
            {
                _logger.LogError("User " + theoDoi.UserID + " gặp lỗi khi tạo mới theo dõi truyện có ID " + theoDoi.TruyenID + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateTheoDoi" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheoDoi(int id, [FromBody] TheoDoiForUpdateDto theoDoi)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

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
            catch(Exception ex)
            {
                _logger.LogError("User " + theoDoi.UserID + " gặp lỗi khi cập nhật theo dõi truyện có ID " + theoDoi.TruyenID + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateTheoDoi" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheoDoi(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

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
            catch(Exception ex)
            {
                _logger.LogError("Gặp lỗi khi xóa theo dõi có ID " + id + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteTheoDoi" });
            }
        }

        [HttpDelete("deleteforuser")]
        public async Task<IActionResult> DeleteTheoDoiForUser([FromQuery] TheoDoiParameters theoDoiParameters)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var theoDoi = await _repository.TheoDoi.GetTheoDoiByUserIdAndTruyenIdAsync(theoDoiParameters.UserID, theoDoiParameters.TruyenID);
                if (theoDoi == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID TheoDoi không tồn tại" });
                }

                ResponseDetails response = _repository.TheoDoi.DeleteTheoDoi(theoDoi);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Gặp lỗi khi xóa theo dõi có userID " + theoDoiParameters.UserID + " và truyện ID " + theoDoiParameters.TruyenID + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteTheoDoiForUser" });
            }
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetTruyenByTheoDoiForPagination([FromQuery] TheoDoiParameters theoDoiParameters)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);
            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });


            if (theoDoiParameters.GetAll)
            {
                var truyens = await _repository.TheoDoi.GetTruyenByTheoDoiForPagination(theoDoiParameters);

                var metadata = new
                {
                    truyens.TotalCount,
                    truyens.PageSize,
                    truyens.CurrentPage,
                    truyens.TotalPages,
                    truyens.HasNext,
                    truyens.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(truyens);
            }
            else return BadRequest("wrong request to get theo doi pagination");
        }
    }
}
