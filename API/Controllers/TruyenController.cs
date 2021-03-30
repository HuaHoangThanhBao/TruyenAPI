using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http.Headers;

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
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllTruyens" });
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
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTruyenById" });
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
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTacGiaByDetails" });
            }
        }

        [HttpPost]
        public IActionResult CreateTruyen([FromForm] TruyenForCreationDto truyen)
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

                if (truyen.HinhAnh == null)
                {
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "File hình ảnh bị trống" });
                }

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (truyen.HinhAnh.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(truyen.HinhAnh.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    //var dbPath = Path.Combine(folderName, fileName);

                    var truyenParse = new Truyen() { 
                        TacGiaID = truyen.TacGiaID,
                        TenTruyen = truyen.TenTruyen, 
                        MoTa = truyen.MoTa,
                        HinhAnh = fileName
                    };

                    var truyenEntity = _mapper.Map<Truyen>(truyenParse);

                    var response = _repository.Truyen.CreateTruyen(truyenParse);
                    if (response.StatusCode == ResponseCode.Success)
                    {
                        _repository.Save();
                    }
                    else return BadRequest(response);

                    var createdTruyen = _mapper.Map<TruyenDto>(truyenEntity);

                    return Ok(createdTruyen);
                }
                else
                {
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "File hình ảnh bị trống" });
                }
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateTruyen" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTruyen(int id, [FromForm] TruyenForUpdateDto truyen)
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

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (truyen.HinhAnh.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(truyen.HinhAnh.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    //var dbPath = Path.Combine(folderName, fileName);

                    _mapper.Map(truyen, truyenEntity);

                    truyenEntity.HinhAnh = fileName;

                    ResponseDetails response = _repository.Truyen.UpdateTruyen(truyenEntity);

                    if (response.StatusCode == ResponseCode.Success)
                    {
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
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateTruyen" });
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

                ResponseDetails response = _repository.Truyen.DeleteTruyen(truyen);

                if (response.StatusCode == ResponseCode.Success)
                    _repository.Save();

                return Ok(response);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteTruyen" });
            }
        }
    }
}
