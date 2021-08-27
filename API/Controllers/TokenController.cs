using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.Service;
using CoreLibrary.Helpers;
using CoreLibrary.Models;
using DataAccessLayer;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private readonly ITokenService _tokenService;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _config;

        public TokenController(IRepositoryWrapper repository, ITokenService tokenService, ILoggerManager logger, IConfiguration config)
        {
            _repository = repository;
            _tokenService = tokenService;
            _logger = logger;
            _config = config;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

            if (tokenApiModel is null)
            {
                return BadRequest(new ResponseDetails() { Message = "Token body truyền vào không hợp lệ", StatusCode = ResponseCode.Error });
            }
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenApiModel.AccessToken, _config);

            if(principal == null)
            {
                return BadRequest(new ResponseDetails() { Message= "Token không được cấp quyền", StatusCode = ResponseCode.Error });
            }
            
            //Lấy claim value
            var userID = principal.Claims?.FirstOrDefault(x => x.Type.Equals(NamePars.ClaimSid, StringComparison.OrdinalIgnoreCase))?.Value;

            var user = await _repository.User.GetUserByIDAsync(userID);
            if (user == null || user.RefreshToken != tokenApiModel.RefreshToken)
            {
                return BadRequest(new ResponseDetails() { Message = "các giá trị token không khớp với dữ liệu trong database", StatusCode = ResponseCode.Error });
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims, _config);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            ResponseDetails response = _repository.User.UpdateUserRefreshToken(user, newRefreshToken, null);
            if (response.StatusCode == ResponseCode.Success)
            {
                _repository.Save();
            }
            else
            {
                _logger.LogError($"Lỗi khi cấp refresh token khi yêu cầu cấp lại token hết hạn cho user với id {user.UserID}");
            }

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> Revoke(TokenApiModel tokenApiModel)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(Request.Headers[NamePars.APIKeyStr]);

            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

            if (tokenApiModel is null)
            {
                return BadRequest(new ResponseDetails() { Message = "Token body truyền vào không hợp lệ", StatusCode = ResponseCode.Error });
            }
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenApiModel.AccessToken, _config);

            if (principal == null)
            {
                return BadRequest(new ResponseDetails() { Message = "Token không được cấp quyền", StatusCode = ResponseCode.Error });
            }

            var userID = principal.Claims?.FirstOrDefault(x => x.Type.Equals(NamePars.ClaimSid, StringComparison.OrdinalIgnoreCase))?.Value;

            var user = await _repository.User.GetUserByIDAsync(userID);
            if (user == null || user.RefreshToken != tokenApiModel.RefreshToken)
            {
                return BadRequest(new ResponseDetails() { Message = "các giá trị token không khớp với dữ liệu trong database", StatusCode = ResponseCode.Error });
            }

            ResponseDetails response = _repository.User.UpdateUserRefreshToken(user, null, null);
            if (response.StatusCode == ResponseCode.Success)
            {
                _repository.Save();
            }
            else
            {
                _logger.LogError($"Lỗi khi revoke token cho user với id {user.UserID}");
            }
            return Ok();
        }
    }
}