using DataAccessLayer;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoiDungChuongController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public NoiDungChuongController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNoiDungChuongs()
        {
            try
            {
                var noiDungChuongs = await _repository.NoiDungChuong.GetAllNoiDungChuongsAsync();
                var noiDungChuongsResult = _mapper.Map<IEnumerable<NoiDungChuongDto>>(noiDungChuongs);

                return Ok(noiDungChuongsResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllNoiDungChuongs" });
            }
        }

        [HttpPost]
        public IActionResult CreateNoiDungChuong([FromBody] IEnumerable<NoiDungChuongForCreationDto> noiDungChuong)
        {
            try
            {
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
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception xảy ra ở hàm CreateNoiDungChuong" });
            }
        }

        [HttpPut/*("{id}")*/]
        public async Task<IActionResult> UpdateNoiDungChuong([FromBody] NoiDungChuongForUpdateDto noiDungChuong)
        {
            try
            {
                if (noiDungChuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var noiDungChuongEntity = await _repository.NoiDungChuong.GetNoiDungChuongByChuongIdAsync(noiDungChuong.NoiDungChuongID);
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
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateNoiDungChuong" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNoiDungChuong(int id)
        {
            try
            {
                var noiDungChuong = await _repository.NoiDungChuong.GetNoiDungChuongByChuongIdAsync(id);
                if (noiDungChuong == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID Nội dung chương không tồn tại" });
                }

                ResponseDetails response = _repository.NoiDungChuong.DeleteNoiDungChuong(noiDungChuong);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteNoiDungChuong" });
            }
        }
    }
}
