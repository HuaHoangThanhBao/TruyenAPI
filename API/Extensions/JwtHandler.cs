using CoreLibrary.DataTransferObjects;
using CoreLibrary.Helpers;
using CoreLibrary.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly IConfigurationSection _goolgeSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        public JwtHandler(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection($"{NamePars.JwtSettings}");
            _goolgeSettings = _configuration.GetSection("GoogleAuthSettings");
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection($"{NamePars.SecurityKeys}").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser userApp, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.UserID.ToString())
            };

            var roles = await _userManager.GetRolesAsync(userApp);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection($"{NamePars.ValidIssuer}").Value,
                audience: _jwtSettings.GetSection($"{NamePars.ValidAudience}").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection($"{NamePars.ExpireTime}").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }

        public async Task<List<Claim>> GenerateClaims(ApplicationUser userApp, User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(userApp, user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return claims;
        }


        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public async Task<string> GenerateToken(ApplicationUser userApp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(userApp);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _goolgeSettings.GetSection("clientId").Value }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }
    }
}
