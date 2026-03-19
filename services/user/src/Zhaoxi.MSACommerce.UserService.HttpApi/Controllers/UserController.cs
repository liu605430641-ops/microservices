using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;
using Zhaoxi.MSACommerce.UserService.HttpApi.Apis;
using Zhaoxi.MSACommerce.UserService.HttpApi.Models;
using Zhaoxi.MSACommerce.UserService.UseCases.Commands;
using Zhaoxi.MSACommerce.UserService.UseCases.Queries;

namespace Zhaoxi.MSACommerce.UserService.HttpApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(IServiceClient<IVerificationApi> client) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]GetUserQuery request)
    {
        var result = await Sender.Send(request);

        return ReturnResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand request)
    {
        var result = await Sender.Send(request);

        return ReturnResult(result);
    }
    
    [HttpGet("test")]
    [Authorize]
    public async Task<IActionResult> ayth([FromServices] IUser user)
    {
        return Ok(user);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserDto userDto)
    {
        var response = await client.ServiceApi.VerifySmsCodeAsync(userDto.Phone, userDto.Code);
        if (!response.IsSuccessStatusCode) return BadRequest(response.Error.Content);
        
        var result = await Sender.Send(new CreateUserCommand(userDto.Username, userDto.Password, userDto.Phone));
        
        return ReturnResult(result);
    }

}
