﻿using System.Collections.Generic;
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
    public class ChuongController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public ChuongController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChuongs()
        {
            try
            {
                var chuongs = await _repository.Chuong.GetAllChuongsAsync();
                var chuongsResult = _mapper.Map<IEnumerable<ChuongDto>>(chuongs);

                return Ok(chuongsResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllChuongs" });
            }
        }

        [HttpGet("{id}", Name = "ChuongById")]
        public async Task<IActionResult> GetChuongById(int id)
        {
            try
            {
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

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetChuongByDetails(int id)
        {
            try
            {
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

        [HttpPost]
        public IActionResult CreateChuong([FromBody] IEnumerable<ChuongForCreationDto> chuong)
        {
            try
            {
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChuong(int id, [FromBody] ChuongForUpdateDto chuong)
        {
            try
            {
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChuong(int id)
        {
            try
            {
                var Chuong = await _repository.Chuong.GetChuongByIdAsync(id);
                if (Chuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID truyện không tồn tại" });
                }

                ResponseDetails response = _repository.Chuong.DeleteChuong(Chuong);

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
