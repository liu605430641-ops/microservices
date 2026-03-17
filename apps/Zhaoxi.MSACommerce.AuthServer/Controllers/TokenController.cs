using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.AuthServer.Services;

namespace Zhaoxi.MSACommerce.AuthServer.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController(IIdentityService identityService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(string username, string password)
    {
        var result = await identityService.GetAccessTokenAsync(username, password);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { errors = result.Errors });
    }
}
