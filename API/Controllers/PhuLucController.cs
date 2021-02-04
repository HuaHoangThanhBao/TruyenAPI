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
                throw new Exception("Exception occured when implement GetAllPhuLucs function");
            }
        }

        [HttpGet("{id}", Name = "PhuLucById")]
        public async Task<IActionResult> GetPhuLucById(int id)
        {
            try
            {
                var phuLuc = await _repository.PhuLuc.GetPhuLucByIdAsync(id);
                if (phuLuc == null)
                {
                    return NotFound();
                }
                else
                {
                    var phuLucResult = _mapper.Map<PhuLucDto>(phuLuc);
                    return Ok(phuLucResult);
                }
            }
            catch
            {
                throw new Exception("Exception occured when implement GetPhuLucById function");
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
                    return BadRequest("PhuLuc object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var phuLucEntity = _mapper.Map<IEnumerable<PhuLuc>>(phuLuc);

                var created = _repository.PhuLuc.CreatePhuLuc(phuLucEntity);
                if (created)
                {
                    _repository.Save();
                }
                else return BadRequest(new ErrorDetails()
                {
                    StatusCode = Response.StatusCode,
                    Message = "Mã truyện hoặc thể loại không hợp lệ"
                });

                var createdPhuLuc = _mapper.Map<IEnumerable<PhuLucDto>>(phuLucEntity);

                return Ok(createdPhuLuc);
            }
            catch
            {
                throw new Exception("Exception occured when implement CreatePhuLuc function");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePhuLuc(int id, [FromBody] PhuLucForUpdateDto phuLuc)
        {
            try
            {
                if (phuLuc == null)
                {
                    return BadRequest("PhuLuc object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var phuLucEntity = await _repository.PhuLuc.GetPhuLucByIdAsync(id);
                if (phuLucEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(phuLuc, phuLucEntity);

                bool updateStatus = _repository.PhuLuc.UpdatePhuLuc(phuLucEntity);

                if (updateStatus)
                {
                    _repository.Save();
                }
                else return BadRequest(new ErrorDetails()
                {
                    StatusCode = Response.StatusCode,
                    Message = "Mã truyện hoặc thể loại không hợp lệ"
                });

                return Ok();
            }
            catch
            {
                throw new Exception("Exception occured when implement UpdatePhuLuc function");
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
                    return NotFound();
                }

                //if (_repository.Account.AccountsByOwner(id).Any())
                //{
                //    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                //}

                _repository.PhuLuc.DeletePhuLuc(phuLuc);

                _repository.Save();

                return NoContent();
            }
            catch
            {
                throw new Exception("Exception occured when implement DeletePhuLuc function");
            }
        }
    }
}
