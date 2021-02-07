using System.Collections.Generic;
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
    public class BinhLuanController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public BinhLuanController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBinhLuans()
        {
            try
            {
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

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetBinhLuanByDetails(int id)
        {
            try
            {
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
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTacGiaByDetails" });
            }
        }

        [HttpPost]
        public IActionResult CreateBinhLuan([FromBody] BinhLuanForCreationDto binhLuan)
        {
            try
            {
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
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateBinhLuan" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBinhLuan(int id, [FromBody] BinhLuanForUpdateDto binhLuan)
        {
            try
            {
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
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateBinhLuan" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBinhLuan(int id)
        {
            try
            {
                var binhLuan = await _repository.BinhLuan.GetBinhLuanByIdAsync(id);
                if (binhLuan == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID Bình luận không tồn tại" });
                }

                //if (_repository.Account.AccountsByOwner(id).Any())
                //{
                //    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                //}

                ResponseDetails response = _repository.BinhLuan.DeleteBinhLuan(binhLuan);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteBinhLuan" });
            }
        }
    }
}
