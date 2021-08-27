using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Service
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
    }
}
