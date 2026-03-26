using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.PaymentService.UseCases.Commands;
using Zhaoxi.MSACommerce.PaymentService.UseCases.Queries;

namespace Zhaoxi.MSACommerce.PaymentService.HttpApi.Controllers;

[Route("api/pay")]
[ApiController]
public class PaymentController : ApiControllerBase
{
    [HttpGet("status/{orderId:long}")]
    [Authorize]
    public async Task<IActionResult> Get(long orderId)
    {
        var result = await Sender.Send(new GetPayStatusQuery(orderId));
        return ReturnResult(result);
    }
    
    [HttpPost("{orderId:long}")]
    [Authorize]
    public async Task<IActionResult> Create(long orderId)
    {
        var result = await Sender.Send(new CreatePayLogCommand(orderId));
        if (!result.IsSuccess) return ReturnResult(result);

        var payUrl = $"{Request.Headers["Origin"]}{Url.Action("UpdatePayed", new { id = result.Value })}";
        
        return Ok(new { payUrl });
    }
    
    // PUT: http://111/api/pay/支付流水ID
    // 微信平台==》访问==》传更多的支付信息
    [HttpGet("{id:long}", Name = "UpdatePayed")]
    public async Task<IActionResult> UpdatePayed(long id)
    {
        var result = await Sender.Send(new UpdatePayedStatusCommand(id));
        return ReturnResult(result);
    }
}
