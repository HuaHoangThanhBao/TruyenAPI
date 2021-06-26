using System.Collections.Generic;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Helpers;
using CoreLibrary.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpGet("{key}")]
        public async Task<IActionResult> GetAllBinhLuans(string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var binhLuans = await _repository.BinhLuan.GetAllBinhLuansAsync();
                var binhLuansResult = _mapper.Map<IEnumerable<BinhLuanDto>>(binhLuans);

                return Ok(binhLuansResult);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetAllBinhLuans" });
            }
        }

        [HttpGet("{id}/{key}", Name = "BinhLuanById")]
        public async Task<IActionResult> GetBinhLuanById(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

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

        [HttpGet("{id}/{key}/details")]
        public async Task<IActionResult> GetBinhLuanByDetails(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

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
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetBinhLuanByDetails" });
            }
        }

        [HttpPost("{key}")]
        public IActionResult CreateBinhLuan(string key, [FromBody] BinhLuanForCreationDto binhLuan)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

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

        [HttpPut("{id}/{key}")]
        public async Task<IActionResult> UpdateBinhLuan(int id, string key, [FromBody] BinhLuanForUpdateDto binhLuan)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

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

        [HttpDelete("{id}/{key}")]
        public async Task<IActionResult> DeleteBinhLuan(int id, string key)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var binhLuan = await _repository.BinhLuan.GetBinhLuanByIdAsync(id);
                if (binhLuan == null)
                {
                    return NotFound(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "ID Bình luận không tồn tại" });
                }

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

        [HttpGet]
        public IActionResult GetBinhLuanForPagination([FromQuery] BinhLuanParameters binhLuanParameters)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(binhLuanParameters.APIKey);
            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });


            if (binhLuanParameters.LastestUpdate)
            {
                var binhLuans = _repository.BinhLuan.GetBinhLuanLastestForPagination(binhLuanParameters);

                var metadata = new
                {
                    binhLuans.TotalCount,
                    binhLuans.PageSize,
                    binhLuans.CurrentPage,
                    binhLuans.TotalPages,
                    binhLuans.HasNext,
                    binhLuans.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(binhLuans);
            }
            else if (binhLuanParameters.Sorting && binhLuanParameters.TruyenID > 0)
            {
                var binhLuans = _repository.BinhLuan.GetBinhLuanOfTruyenForPagination(binhLuanParameters.TruyenID, binhLuanParameters);

                var metadata = new
                {
                    binhLuans.TotalCount,
                    binhLuans.PageSize,
                    binhLuans.CurrentPage,
                    binhLuans.TotalPages,
                    binhLuans.HasNext,
                    binhLuans.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(binhLuans);
            }
            else if (binhLuanParameters.Sorting && binhLuanParameters.ChuongID > 0)
            {
                var binhLuans = _repository.BinhLuan.GetBinhLuanOfChuongForPagination(binhLuanParameters.ChuongID, binhLuanParameters);

                var metadata = new
                {
                    binhLuans.TotalCount,
                    binhLuans.PageSize,
                    binhLuans.CurrentPage,
                    binhLuans.TotalPages,
                    binhLuans.HasNext,
                    binhLuans.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(binhLuans);
            }
            else
            {
                if (binhLuanParameters.GetAll)
                {
                    var binhLuans = _repository.BinhLuan.GetBinhLuanForPagination(binhLuanParameters);

                    var metadata = new
                    {
                        binhLuans.TotalCount,
                        binhLuans.PageSize,
                        binhLuans.CurrentPage,
                        binhLuans.TotalPages,
                        binhLuans.HasNext,
                        binhLuans.HasPrevious
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    return Ok(binhLuans);
                }
                else return BadRequest("wrong request to get binhluan pagination");
            }
        }
    }
}
