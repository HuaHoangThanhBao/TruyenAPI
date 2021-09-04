using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using Newtonsoft.Json;
using CoreLibrary.Helpers;
using Microsoft.AspNetCore.Cors;
using System.Text.RegularExpressions;
using System.Text;
using LoggerService;
using System;

namespace API.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TruyenController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private IMapper _mapper;

        public TruyenController(IRepositoryWrapper repository, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTruyens()
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

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
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

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
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

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

        [HttpGet("{id}/danhgiasao")]
        public IActionResult GetSoSaoDanhGiaOfTruyen(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

                var soSao = _repository.Truyen.GetDanhGiaSaoOfTruyenAsync(id);

                return Ok(soSao);
            }
            catch
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm GetTacGiaByDetails" });
            }
        }

        [HttpPost]
        public IActionResult CreateTruyen([FromBody] IEnumerable<TruyenForCreationDto> truyen)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

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

                var createdTruyen = _mapper.Map<IEnumerable<TruyenDto>>(truyenEntity);

                _logger.LogInfo("Thêm mới danh sách truyện thành công");

                return Ok(createdTruyen);
            }
            catch(Exception ex)
            {
                _logger.LogError("Lỗi khi create list truyện: " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm CreateTruyen" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTruyen(int id, [FromBody] TruyenForUpdateDto truyen)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

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
            catch(Exception ex)
            {
                _logger.LogError("Lỗi khi update truyện " + id + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm UpdateTruyen" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruyen(int id)
        {
            try
            {
                var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

                if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                    return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

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
            catch(Exception ex)
            {
                _logger.LogError("Lỗi khi xóa truyện " + id + ": " + ex);
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = "Lỗi execption ở hàm DeleteTruyen" });
            }
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetTruyenForPagination([FromQuery] TruyenParameters truyenParameters)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);
            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });


            if (truyenParameters.LastestUpdate)
            {
                var truyens = await _repository.Truyen.GetTruyenLastestUpdateForPagination(truyenParameters);

                var metadata = new
                {
                    truyens.TotalCount,
                    truyens.PageSize,
                    truyens.CurrentPage,
                    truyens.TotalPages,
                    truyens.HasNext,
                    truyens.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(truyens);
            }
            else if (truyenParameters.TopView)
            {
                var truyens = await _repository.Truyen.GetTopViewForPagination(truyenParameters);

                var metadata = new
                {
                    truyens.TotalCount,
                    truyens.PageSize,
                    truyens.CurrentPage,
                    truyens.TotalPages,
                    truyens.HasNext,
                    truyens.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(truyens);
            }
            else if (truyenParameters.Sorting && truyenParameters.TheLoaiID > 0)
            {
                var truyens = await _repository.Truyen.GetTruyenOfTheLoaiForPagination(truyenParameters.TheLoaiID, truyenParameters);

                var metadata = new
                {
                    truyens.TotalCount,
                    truyens.PageSize,
                    truyens.CurrentPage,
                    truyens.TotalPages,
                    truyens.HasNext,
                    truyens.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(truyens);
            }
            else if (truyenParameters.Sorting && truyenParameters.TenTruyen != null)
            {
                List<Truyen> truyenUnsigned = new List<Truyen>();

                var all = await _repository.Truyen.FindTruyenForPagination();

                string unsignTenTruyen = "";
                string unsignTruyenParam = ConvertToUnSign(truyenParameters.TenTruyen);

                foreach (var item in all)
                {
                    unsignTenTruyen = ConvertToUnSign(item.TenTruyen);
                    if (unsignTenTruyen.Contains(unsignTruyenParam))
                    {
                        truyenUnsigned.Add(new Truyen()
                        {
                            TruyenID = item.TruyenID,
                            TenTruyen = item.TenTruyen,
                            HinhAnh = item.HinhAnh
                        });
                    }
                }
                
                return Ok(truyenUnsigned);
            }
            else
            {
                if (truyenParameters.GetAll)
                {
                    var truyens = await _repository.Truyen.GetTruyenForPagination(truyenParameters);

                    var metadata = new
                    {
                        truyens.TotalCount,
                        truyens.PageSize,
                        truyens.CurrentPage,
                        truyens.TotalPages,
                        truyens.HasNext,
                        truyens.HasPrevious
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    return Ok(truyens);
                }
                else return BadRequest("wrong request to get truyen pagination");
            }
        }

        private string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }
    }
}
