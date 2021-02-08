using DataAccessLayer;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoiDungTruyenController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public NoiDungTruyenController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNoiDungTruyens()
        {
            try
            {
                var noiDungTruyens = await _repository.NoiDungTruyen.GetAllNoiDungTruyensAsync();
                var noiDungTruyensResult = _mapper.Map<IEnumerable<NoiDungTruyenDto>>(noiDungTruyens);

                return Ok(noiDungTruyensResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllNoiDungTruyens" });
            }
        }

        [HttpPost]
        public IActionResult CreateNoiDungTruyen([FromForm] NoiDungTruyenForCreationDto model)
        {
            try
            {
                if(model.HinhAnh == null)
                {
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "File hình ảnh bị trống" });
                }

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (model.HinhAnh.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(model.HinhAnh.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    //var dbPath = Path.Combine(folderName, fileName);
                    var noiDung = new NoiDungTruyen() { TruyenID = model.TruyenID, HinhAnh = fileName };

                    ResponseDetails response = _repository.NoiDungTruyen.CreateNoiDungTruyen(noiDung);
                    if (response.StatusCode == ResponseCode.Success)
                    {
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            model.HinhAnh.CopyTo(stream);
                        }
                        _repository.Save();
                    }
                    else return BadRequest(response);

                    return Ok(response);
                }
                else
                {
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "File hình ảnh bị trống" });
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception xảy ra ở hàm CreateNoiDungTruyen" });
            }
        }

        [HttpPut/*("{id}")*/]
        public async Task<IActionResult> UpdateNoiDungTruyen([FromForm] NoiDungTruyenForUpdateDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Các trường dữ liệu chưa đúng" });
                }

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (model.HinhAnh.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(model.HinhAnh.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    //var dbPath = Path.Combine(folderName, fileName);

                    var noiDungTruyenEntity = await _repository.NoiDungTruyen.GetNoiDungTruyenByTruyenIdAsync(model.NoiDungTruyenID, model.TruyenID);
                    if (noiDungTruyenEntity == null)
                    {
                        return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "TruyenID hoặc NoiDungID không tồn tại" });
                    }
                    noiDungTruyenEntity.HinhAnh = fileName;

                    ResponseDetails response = _repository.NoiDungTruyen.UpdateNoiDungTruyen(noiDungTruyenEntity);
                    if (response.StatusCode == ResponseCode.Success)
                    {
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            model.HinhAnh.CopyTo(stream);
                        }
                        _repository.Save();
                    }
                    else return BadRequest(response);

                    return Ok(response);
                }
                else
                {
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "File hình ảnh bị trống" });
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception xảy ra ở hàm CreateNoiDungTruyen" });
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
