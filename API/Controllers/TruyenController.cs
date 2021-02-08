using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruyenController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public TruyenController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTruyens()
        {
            try
            {
                var truyens = await _repository.Truyen.GetAllTruyensAsync();
                var truyensResult = _mapper.Map<IEnumerable<TruyenDto>>(truyens);

                return Ok(truyensResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception ở hàm GetAllTruyens" });
            }
        }

        [HttpGet("{id}", Name = "TruyenById")]
        public async Task<IActionResult> GetTruyenById(int id)
        {
            try
            {
                var truyen = await _repository.Truyen.GetTruyenByIdAsync(id);
                if (truyen == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Truyện không tồn tại" });
                }
                else
                {
                    var truyenResult = _mapper.Map<TruyenDto>(truyen);
                    return Ok(truyenResult);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception ở hàm GetTruyenById" });
            }
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetTruyenByDetails(int id)
        {
            try
            {
                var truyen = await _repository.Truyen.GetTruyenByDetailAsync(id);

                if (truyen == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Truyện không tồn tại" });
                }
                else
                {
                    //var truyenResult = _mapper.Map<TruyenDto>(truyen);
                    return Ok(truyen);
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception ở hàm GetTacGiaByDetails" });
            }
        }

        [HttpPost]
        public IActionResult CreateTruyen([FromBody] IEnumerable<TruyenForCreationDto> truyen)
        {
            try
            {
                if (truyen == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var truyenEntity = _mapper.Map<IEnumerable<Truyen>>(truyen);

                var response = _repository.Truyen.CreateTruyen(truyenEntity);
                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                var createdTruyen= _mapper.Map<IEnumerable<TruyenDto>>(truyenEntity);

                return Ok(createdTruyen);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception ở hàm CreateTruyen" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTruyen(int id, [FromBody] TruyenForUpdateDto truyen)
        {
            try
            {
                if (truyen == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var truyenEntity = await _repository.Truyen.GetTruyenByIdAsync(id);
                if (truyenEntity == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Truyện không tồn tại" });
                }

                _mapper.Map(truyen, truyenEntity);

                ResponseDetails response = _repository.Truyen.UpdateTruyen(truyenEntity);

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception ở hàm UpdateTruyen" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruyen(int id)
        {
            try
            {
                var truyen = await _repository.Truyen.GetTruyenByIdAsync(id);
                if (truyen == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID truyện không tồn tại" });
                }

                //if (_repository.Account.AccountsByOwner(id).Any())
                //{
                //    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                //}

                ResponseDetails response = _repository.Truyen.DeleteTruyen(truyen);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception ở hàm DeleteTruyen" });
            }
        }
    }
}
