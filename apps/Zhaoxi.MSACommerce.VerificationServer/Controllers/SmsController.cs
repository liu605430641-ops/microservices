using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.VerificationServer.Services;

namespace Zhaoxi.MSACommerce.VerificationServer.Controllers;

[Route("api/verification/sms")]
[ApiController]
public class SmsController(ISmsService smsService) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendCode(string phoneNumber)
    {
        var result = await smsService.SendCodeAsync(phoneNumber);
        return result.IsSuccess ? Ok() : BadRequest(new {result.Errors});
    }
    
    [HttpPost("verify")]
    public async Task<IActionResult> VerifyCode(string phoneNumber, string code)
    {
        var result = await smsService.VerifyCodeAsync(phoneNumber, code);
        return result.IsSuccess ? Ok() : BadRequest(new {result.Errors});
    }
}