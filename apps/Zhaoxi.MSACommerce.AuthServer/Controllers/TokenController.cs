using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.AuthServer.Apis;
using Zhaoxi.MSACommerce.AuthServer.Services;
using Zhaoxi.MSACommerce.LoadBalancer;

namespace Zhaoxi.MSACommerce.AuthServer.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController(ITokenService tokenService, IServiceClient<IUserServiceApi> client, IConnectionMultiplexer redis) : ControllerBase
{
    private readonly IDatabase _redisDb = redis.GetDatabase();
    
    [HttpGet]
    public async Task<IActionResult> Get(string username, string password)
    {
        // 验证用户名和密码
        var response = await client.ServiceApi.GetUserAsync(username, password);
        if (!response.IsSuccessStatusCode)
        {
            return Unauthorized();
        }
        var user = response.Content;
        
        // 生成用户声明
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.MobilePhone, user.Phone ?? "")
        };
        
        var accessToken  = tokenService.GenerateAccessToken(claims);
        var refreshToken  = tokenService.GenerateRefreshToken();
        
        // 保存刷新令牌
        var key = $"user:refreshToken:{username}";
        await _redisDb.StringSetAsync(key, refreshToken, TimeSpan.FromDays(7), When.Always);

        return Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPut("refresh")]
    public async Task<IActionResult> Refresh(string accessToken, string refreshToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name;

        // 需不需要二次鉴权...
        // 查数据库……
        
        // 检查刷新令牌是否过期
        var key = $"user:refreshToken:{username}";
        var dbRefreshToken = await _redisDb.StringGetAsync(key);
        if (dbRefreshToken.IsNull || dbRefreshToken != refreshToken)
        {
            return BadRequest("无效的请求");
        }
        
        var newAccessToken = tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        // 保存新的刷新令牌（保持原有TTL）
        await _redisDb.StringSetAsync(key, newRefreshToken, keepTtl: true);
        
        return Ok(new
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
    
    [HttpDelete, Authorize]
    public async Task<IActionResult> Revoke()
    {
        var username = User.Identity.Name;
        var key = $"user:refreshToken:{username}";
        await _redisDb.KeyDeleteAsync(key);
        return NoContent();
    }
}
