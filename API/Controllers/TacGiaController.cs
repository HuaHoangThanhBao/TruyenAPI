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
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TacGiaController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public TacGiaController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTacGias()
        {
            try
            {
                var tacGias = await _repository.TacGia.GetAllTacGiasAsync();
                var tacGiasResult = _mapper.Map<IEnumerable<TacGiaDto>>(tacGias);

                return Ok(tacGiasResult);
            }
            catch
            {
                throw new Exception("Exception occured when implement GetAllTacGias function");
            }
        }

        [HttpGet("{id}", Name = "TacGiaById")]
        public async Task<IActionResult> GetTacGiaById(Guid id)
        {
            try
            {
                var tacGia = await _repository.TacGia.GetTacGiaByIdAsync(id);
                if (tacGia == null)
                {
                    return NotFound();
                }
                else
                {
                    var ownerResult = _mapper.Map<TacGiaDto>(tacGia);
                    return Ok(ownerResult);
                }
            }
            catch
            {
                throw new Exception("Exception occured when implement GetTacGiaById function");
            }
        }

        [HttpGet("{id}/account")]
        public async Task<IActionResult> GetTacGiaByDetails(Guid id)
        {
            try
            {
                var tacGia = await _repository.TacGia.GetTacGiaByDetailAsync(id);

                if(tacGia == null)
                {
                    return NotFound();
                }
                else
                {
                    var ownerResult = _mapper.Map<TacGiaDto>(tacGia);
                    return Ok(ownerResult);
                }
            }
            catch
            {
                throw new Exception("Exception occured when implement GetTacGiaByDetails function");
            }
        }

        [HttpPost]
        public IActionResult CreateTacGia([FromBody] IEnumerable<TacGiaForCreationDto> tacGia)
        {
            try
            {
                if(tacGia == null)
                {
                    return BadRequest("TacGia object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var tacGiaEntity = _mapper.Map<IEnumerable<TacGia>>(tacGia);

                var founded = _repository.TacGia.CreateTacGia(tacGiaEntity);
                if (founded == null)
                {
                    _repository.Save();
                }
                else return BadRequest(new ErrorDetails()
                {
                    StatusCode = Response.StatusCode,
                    Message = founded.TenTacGia
                });

                var createdOwner = _mapper.Map<IEnumerable<TacGiaDto>>(tacGiaEntity);

                return Ok(createdOwner);
            }
            catch
            {
                throw new Exception("Exception occured when implement CreateTacGia function");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTacGia(Guid id, [FromBody]TacGiaForUpdateDto owner)
        {
            try
            {
                if (owner == null)
                {
                    return BadRequest("TacGia object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var tacGiaEntity = await _repository.TacGia.GetTacGiaByIdAsync(id);
                if (tacGiaEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(owner, tacGiaEntity);

                bool updateStatus = _repository.TacGia.UpdateTacGia(tacGiaEntity);

                if (updateStatus)
                    _repository.Save();
                else return BadRequest(new ErrorDetails()
                {
                    StatusCode = Response.StatusCode,
                    Message = "Tên tác giả bị trùng"
                });

                return Ok();
            }
            catch
            {
                throw new Exception("Exception occured when implement UpdateTacGia function");
            }
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOwner(int id)
        //{
        //    try
        //    {
        //        var owner = await _repository.Owner.GetOwnerByIdAsync(id);
        //        if(owner == null)
        //        {
        //            return NotFound();
        //        }

        //        if (_repository.Account.AccountsByOwner(id).Any())
        //        {
        //            return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
        //        }

        //        _repository.Owner.DeleteOwner(owner);
                
        //        await _repository.SaveAsync();

        //        return NoContent();
        //    }
        //    catch
        //    {
        //        throw new Exception("Exception occured when implement DeleteOwner function");
        //    }
        //}
    }
}
