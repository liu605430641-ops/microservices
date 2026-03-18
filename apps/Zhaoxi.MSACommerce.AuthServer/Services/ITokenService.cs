using System.Security.Claims;
using Zhaoxi.MSACommerce.SharedKernel.Result;

namespace Zhaoxi.MSACommerce.AuthServer.Services;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
