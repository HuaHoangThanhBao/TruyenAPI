using API.Extensions;
using CoreLibrary.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Service
{
    public class TokenService : ITokenService
    {
        public string GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config[$"{NamePars.JwtSettings}:{NamePars.SecurityKeys}"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _config[$"{NamePars.JwtSettings}:{NamePars.ValidIssuer}"],
                audience: _config[$"{NamePars.JwtSettings}:{NamePars.ValidAudience}"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config[$"{NamePars.JwtSettings}:{NamePars.ExpireTime}"])),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config[$"{NamePars.JwtSettings}:{NamePars.SecurityKeys}"])),
                ValidateLifetime = false, //here we are saying that we don't care about the token's expiration date


                ValidIssuer = _config[$"{NamePars.JwtSettings}:{NamePars.ValidIssuer}"],
                ValidAudience = _config[$"{NamePars.JwtSettings}:{NamePars.ValidAudience}"]

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = new ClaimsPrincipal();
            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
            }
            catch
            {
                principal = null;
            }
            return principal;
        }
    }
}
