﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Helpers;
using CoreLibrary.Models;
using DataAccessLayer;
using LoggerService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class BinhLuanController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private IMapper _mapper;

        public BinhLuanController(IRepositoryWrapper repository, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBinhLuans()
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var binhLuans = await _repository.BinhLuan.GetAllBinhLuansAsync();
                var binhLuansResult = _mapper.Map<IEnumerable<BinhLuanDto>>(binhLuans);

                return Ok(binhLuansResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllBinhLuans" });
            }
        }

        [HttpGet("{id}", Name = "BinhLuanById")]
        public async Task<IActionResult> GetBinhLuanById(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var binhLuan = await _repository.BinhLuan.GetBinhLuanByIdAsync(id);
                if (binhLuan == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Bình luận không tồn tại" });
                }
                else
                {
                    var binhLuanResult = _mapper.Map<BinhLuanDto>(binhLuan);
                    return Ok(binhLuanResult);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetBinhLuanById" });
            }
        }

        [HttpGet("{id}/binhluansbyuser")]
        public async Task<IActionResult> GetBinhLuansByUserId(Guid id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var binhLuan = await _repository.BinhLuan.GetBinhLuanByUserIdAsync(id);
                if (binhLuan == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Bình luận không tồn tại" });
                }
                else
                {
                    return Ok(binhLuan);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetBinhLuansByUserId" });
            }
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetBinhLuanByDetails(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var binhLuan = await _repository.BinhLuan.GetBinhLuanByDetailAsync(id);

                if (binhLuan == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Bình luận không tồn tại" });
                }
                else
                {
                    //var BinhLuanResult = _mapper.Map<BinhLuanDto>(BinhLuan);
                    return Ok(binhLuan);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetBinhLuanByDetails" });
            }
        }

        [HttpPost]
        public IActionResult CreateBinhLuan([FromBody] BinhLuanForCreationDto binhLuan)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (binhLuan == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var binhLuanEntity = _mapper.Map<BinhLuan>(binhLuan);

                var response = _repository.BinhLuan.CreateBinhLuan(binhLuanEntity);
                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                var createdBinhLuan = _mapper.Map<BinhLuanDto>(binhLuanEntity);

                return Ok(createdBinhLuan);
            }
            catch(Exception ex)
            {
                _logger.LogError("Gặp lỗi khi tạo mới bình luận: " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateBinhLuan" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBinhLuan(int id, [FromBody] BinhLuanForUpdateDto binhLuan)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                if (binhLuan == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var binhLuanEntity = await _repository.BinhLuan.GetBinhLuanByIdAsync(id);
                if (binhLuanEntity == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Bình luận không tồn tại" });
                }

                _mapper.Map(binhLuan, binhLuanEntity);

                ResponseDetails response = _repository.BinhLuan.UpdateBinhLuan(binhLuanEntity);

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError("Gặp lỗi khi cập nhật bình luận với ID " + id + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateBinhLuan" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBinhLuan(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var binhLuan = await _repository.BinhLuan.GetBinhLuanByIdAsync(id);
                if (binhLuan == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID Bình luận không tồn tại" });
                }

                ResponseDetails response = _repository.BinhLuan.DeleteBinhLuan(binhLuan);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError("Gặp lỗi khi xóa bình luận với ID " + id + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteBinhLuan" });
            }
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetBinhLuanForPagination([FromQuery] BinhLuanParameters binhLuanParameters)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);
            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });


            if (binhLuanParameters.LastestUpdate)
            {
                var binhLuans = await _repository.BinhLuan.GetBinhLuanLastestForPagination(binhLuanParameters);

                var metadata = new
                {
                    binhLuans.TotalCount,
                    binhLuans.PageSize,
                    binhLuans.CurrentPage,
                    binhLuans.TotalPages,
                    binhLuans.HasNext,
                    binhLuans.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(binhLuans);
            }
            else if (binhLuanParameters.Sorting && binhLuanParameters.TruyenID > 0)
            {
                var binhLuans = await _repository.BinhLuan.GetBinhLuanOfTruyenForPagination(binhLuanParameters.TruyenID, binhLuanParameters);

                var metadata = new
                {
                    binhLuans.TotalCount,
                    binhLuans.PageSize,
                    binhLuans.CurrentPage,
                    binhLuans.TotalPages,
                    binhLuans.HasNext,
                    binhLuans.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(binhLuans);
            }
            else if (binhLuanParameters.Sorting && binhLuanParameters.ChuongID > 0)
            {
                var binhLuans = await _repository.BinhLuan.GetBinhLuanOfChuongForPagination(binhLuanParameters.ChuongID, binhLuanParameters);

                var metadata = new
                {
                    binhLuans.TotalCount,
                    binhLuans.PageSize,
                    binhLuans.CurrentPage,
                    binhLuans.TotalPages,
                    binhLuans.HasNext,
                    binhLuans.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(binhLuans);
            }
            else
            {
                if (binhLuanParameters.GetAll)
                {
                    var binhLuans = await _repository.BinhLuan.GetBinhLuanForPagination(binhLuanParameters);

                    var metadata = new
                    {
                        binhLuans.TotalCount,
                        binhLuans.PageSize,
                        binhLuans.CurrentPage,
                        binhLuans.TotalPages,
                        binhLuans.HasNext,
                        binhLuans.HasPrevious
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    return Ok(binhLuans);
                }
                else return BadRequest("wrong request to get binhluan pagination");
            }
        }
    }
}
