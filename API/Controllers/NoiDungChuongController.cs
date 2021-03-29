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
        public IActionResult CreateNoiDungChuong([FromForm] NoiDungChuongForCreationDto model)
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
                    var noiDung = new NoiDungChuong() { ChuongID = model.ChuongID, HinhAnh = fileName };

                    ResponseDetails response = _repository.NoiDungChuong.CreateNoiDungChuong(noiDung);
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
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception xảy ra ở hàm CreateNoiDungChuong" });
            }
        }

        [HttpPut/*("{id}")*/]
        public async Task<IActionResult> UpdateNoiDungChuong([FromForm] NoiDungChuongForUpdateDto model)
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

                    var noiDungChuongEntity = await _repository.NoiDungChuong.GetNoiDungChuongByChuongIdAsync(model.NoiDungChuongID);
                    if (noiDungChuongEntity == null)
                    {
                        return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "NoiDungChuongID không tồn tại" });
                    }
                    noiDungChuongEntity.HinhAnh = fileName;

                    ResponseDetails response = _repository.NoiDungChuong.UpdateNoiDungChuong(noiDungChuongEntity);
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
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi exception xảy ra ở hàm CreateNoiDungChuong" });
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
