using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Http;
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
                throw new Exception("Exception occured when implement GetAllTruyens function");
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
                    return NotFound();
                }
                else
                {
                    var truyenResult = _mapper.Map<TruyenDto>(truyen);
                    return Ok(truyenResult);
                }
            }
            catch
            {
                throw new Exception("Exception occured when implement GetTruyenById function");
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
        public IActionResult CreateTruyen([FromBody] IEnumerable<TruyenForCreationDto> truyen)
        {
            try
            {
                if (truyen == null)
                {
                    return BadRequest("Truyen object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var truyenEntity = _mapper.Map<IEnumerable<Truyen>>(truyen);

                var founded = _repository.Truyen.CreateTruyen(truyenEntity);
                if (founded == null)
                {
                    _repository.Save();
                }
                else return BadRequest(new ErrorDetails()
                {
                    StatusCode = Response.StatusCode,
                    Message = founded.TenTruyen
                });

                var createdTruyen= _mapper.Map<IEnumerable<TruyenDto>>(truyenEntity);

                return Ok(createdTruyen);
            }
            catch
            {
                throw new Exception("Exception occured when implement CreateTruyen function");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTruyen(int id, [FromBody] TruyenForUpdateDto truyen)
        {
            try
            {
                if (truyen == null)
                {
                    return BadRequest("TacGia object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var truyenEntity = await _repository.Truyen.GetTruyenByIdAsync(id);
                if (truyenEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(truyen, truyenEntity);

                bool updateStatus = _repository.Truyen.UpdateTruyen(truyenEntity);

                if (updateStatus)
                {
                    _repository.Save();
                }
                else return BadRequest(new ErrorDetails()
                {
                    StatusCode = Response.StatusCode,
                    Message = "Tên truyện cập nhật bị trùng"
                });

                return Ok();
            }
            catch
            {
                throw new Exception("Exception occured when implement UpdateTruyen function");
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
                    return NotFound();
                }

                //if (_repository.Account.AccountsByOwner(id).Any())
                //{
                //    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                //}

                _repository.Truyen.DeleteTruyen(truyen);

                _repository.Save();

                return NoContent();
            }
            catch
            {
                throw new Exception("Exception occured when implement DeleteTruyen function");
            }
        }
    }
}
