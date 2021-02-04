﻿using System;
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
    public class TheLoaiController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public TheLoaiController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTheLoais()
        {
            try
            {
                var theLoais = await _repository.TheLoai.GetAllTheLoaisAsync();
                var theLoaisResult = _mapper.Map<IEnumerable<TheLoaiDto>>(theLoais);

                return Ok(theLoaisResult);
            }
            catch
            {
                throw new Exception("Exception occured when implement GetAllTheLoais function");
            }
        }

        [HttpGet("{id}", Name = "TheLoaiById")]
        public async Task<IActionResult> GetTheLoaiById(int id)
        {
            try
            {
                var theLoai = await _repository.TheLoai.GetTheLoaiByIdAsync(id);
                if (theLoai == null)
                {
                    return NotFound();
                }
                else
                {
                    var theLoaiResult = _mapper.Map<TacGiaDto>(theLoai);
                    return Ok(theLoaiResult);
                }
            }
            catch
            {
                throw new Exception("Exception occured when implement GetTheLoaiById function");
            }
        }

        [HttpGet("{id}/the-loai-details")]
        public async Task<IActionResult> GetTheLoaiByDetails(int id)
        {
            try
            {
                var theLoai = await _repository.TheLoai.GetTheLoaiByDetailAsync(id);

                if (theLoai == null)
                {
                    return NotFound();
                }
                else
                {
                    var theLoaiResult = _mapper.Map<TheLoaiDto>(theLoai);
                    return Ok(theLoaiResult);
                }
            }
            catch
            {
                throw new Exception("Exception occured when implement GetTheLoaiByDetails function");
            }
        }

        [HttpPost]
        public IActionResult CreateTheLoai([FromBody] IEnumerable<TheLoaiForCreationDto> theLoai)
        {
            try
            {
                if (theLoai == null)
                {
                    return BadRequest("TheLoai object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var theLoaiEntity = _mapper.Map<IEnumerable<TheLoai>>(theLoai);

                var founded = _repository.TheLoai.CreateTheLoai(theLoaiEntity);
                if (founded == null)
                {
                    _repository.Save();
                }
                else return BadRequest(new ErrorDetails()
                {
                    StatusCode = Response.StatusCode,
                    Message = founded.TenTheLoai
                });

                var createdTheLoai = _mapper.Map<IEnumerable<TheLoaiDto>>(theLoaiEntity);

                return Ok(createdTheLoai);
            }
            catch
            {
                throw new Exception("Exception occured when implement CreateTheLoai function");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheLoai(int id, [FromBody] TheLoaiForUpdateDto theLoai)
        {
            try
            {
                if (theLoai == null)
                {
                    return BadRequest("TheLoai object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var theLoaiEntity = await _repository.TheLoai.GetTheLoaiByIdAsync(id);
                if (theLoaiEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(theLoai, theLoaiEntity);

                bool updateStatus = _repository.TheLoai.UpdateTheLoai(theLoaiEntity);

                if (updateStatus)
                {
                    _repository.Save();
                }
                else return BadRequest(new ErrorDetails()
                {
                    StatusCode = Response.StatusCode,
                    Message = "Tên thể loại cập nhật bị trùng"
                });

                return Ok();
            }
            catch
            {
                throw new Exception("Exception occured when implement UpdateTacGia function");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheLoai(int id)
        {
            try
            {
                var theLoai = await _repository.TheLoai.GetTheLoaiByIdAsync(id);
                if (theLoai == null)
                {
                    return NotFound();
                }

                if (_repository.PhuLuc.TheLoaisInPhuLuc(id).Any())
                {
                    return BadRequest("Cannot delete this TheLoai. It has related PhuLucs. Delete those PhuLucs first");
                }

                _repository.TheLoai.DeleteTheLoai(theLoai);

                _repository.Save();

                return NoContent();
            }
            catch
            {
                throw new Exception("Exception occured when implement DeleteTheLoai function");
            }
        }
    }
}
