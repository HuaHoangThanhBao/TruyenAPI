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
    public class PhuLucController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public PhuLucController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPhuLucs()
        {
            try
            {
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

        //[HttpGet("{id}/account")]
        //public async Task<IActionResult> GetTacGiaByDetails(Guid id)
        //{
        //    try
        //    {
        //        var tacGia = await _repository.TacGia.GetTacGiaByDetailAsync(id);

        //        if(tacGia == null)
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            var ownerResult = _mapper.Map<TacGiaDto>(tacGia);
        //            return Ok(ownerResult);
        //        }
        //    }
        //    catch
        //    {
        //        throw new Exception("Exception occured when implement GetTacGiaByDetails function");
        //    }
        //}

        [HttpPost]
        public IActionResult CreatePhuLuc([FromBody] IEnumerable<PhuLucForCreationDto> phuLuc)
        {
            try
            {
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
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreatePhuLuc" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePhuLuc(int id, [FromBody] PhuLucForUpdateDto phuLuc)
        {
            try
            {
                if (phuLuc == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin trống" });
                }

                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var phuLucEntity = await _repository.PhuLuc.GetPhuLucByIdAsync(id);
                if (phuLucEntity == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Phụ lục không tồn tại" });
                }

                _mapper.Map(phuLuc, phuLucEntity);

                ResponseDetails response = _repository.PhuLuc.UpdatePhuLuc(phuLucEntity);

                if (response.StatusCode == ResponseCode.Success)
                {
                    _repository.Save();
                }
                else return BadRequest(response);

                return Ok(response);
            }
            catch
            {
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

                //if (_repository.Account.AccountsByOwner(id).Any())
                //{
                //    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                //}

                ResponseDetails response = _repository.PhuLuc.DeletePhuLuc(phuLuc);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeletePhuLuc" });
            }
        }
    }
}
