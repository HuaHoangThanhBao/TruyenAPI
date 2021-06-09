using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using API.Extensions;
using CoreLibrary;
using CoreLibrary.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public AuthController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost("{key}/login")]
        public IActionResult Login(string key, [FromBody] LoginModel user)
        {
            var apiKeyAuthenticate = APICredentialAuth.APIKeyCheck(key);

            if (apiKeyAuthenticate.StatusCode == ResponseCode.Error)
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Exception, Message = apiKeyAuthenticate.Message });

            if (user == null)
            {
                return BadRequest(new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Thông tin user trống" });
            }

            var loginResult = _repository.Authenticate.LogIn(user.TenUser, user.Password);
            if (loginResult.StatusCode == ResponseCode.Success)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(APICredentialAuth.GetJWTKey().Value));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    //issuer: "http://localhost:50504",
                    //audience: "http://localhost:50504",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized(loginResult);
            }
        }
    }
}
