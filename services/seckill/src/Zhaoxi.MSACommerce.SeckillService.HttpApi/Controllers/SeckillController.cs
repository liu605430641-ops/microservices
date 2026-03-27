using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.SeckillService.Core.Enums;
using Zhaoxi.MSACommerce.SeckillService.Infrastructure;
using Zhaoxi.MSACommerce.SeckillService.UseCases;
using Zhaoxi.MSACommerce.SeckillService.UseCases.Commands;
using Zhaoxi.MSACommerce.SeckillService.UseCases.Queries;
using Zhaoxi.MSACommerce.SharedKernel.Result;

namespace Zhaoxi.MSACommerce.SeckillService.HttpApi.Controllers;

[Route("api/seckill")]
[ApiController]
public class SeckillController() : ApiControllerBase
{
    [HttpGet("times")]
    public IActionResult GetSecKillBeginTimes()
    {
        var beginTimes = SecKillDate.GetBeginTimes()
            .Select(t => new { time = t.ToSecKillTime(), display = t.ToString("HH:mm") });
        return Ok(beginTimes);
    }

    // 2024120620
    [HttpGet("list/{time}")]
    public async Task<IActionResult> GetSecKillProductsByTime(string time)
    {
        var result = await Sender.Send(new GetSecKillProductsByTimeQuery(time));
        return ReturnResult(result);
    }

    [HttpGet("{time}/{id:long}")]
    public async Task<IActionResult> GetSecKillProductById(string time, long id)
    {
        var result = await Sender.Send(new GetSecKillProductByIdQuery(time, id));
        if (!result.IsSuccess) return ReturnResult(result);
        return Ok(new { product = result.Value, CurrentTime = DateTime.Now });
    }

    [HttpGet("verifyCode")]
    [Authorize]
    public async Task<IActionResult> GetVerifyCode()
    {
        var result = await Sender.Send(new CreateVerifyCodeCommand(4));
        return result.IsSuccess ? File(result.Value, "image/jpeg") : ReturnResult(result);
    }

    [HttpGet("link/{id:long}")]
    [Authorize]
    public async Task<IActionResult> GetSecKillLink(long id, [FromQuery] string verifyCode)
    {
        var verifyResult = await Sender.Send(new GetVerifyCodeQuery(verifyCode));
        if (!verifyResult.IsSuccess) return ReturnResult(verifyResult);

        var linkResult = await Sender.Send(new CreateSecKillLinkCommand(id));
        return ReturnResult(linkResult);
    }

    [HttpPost("order/{link}/{time}/{id:long}")]
    [Authorize]
    public async Task<IActionResult> CreateSecKillOrder(string link, string time, long id,
        [FromServices] MultiThreadingCreateOrder multiThreadingCreateOrder)
    {
        var linkResult = await Sender.Send(new GetSecKillLinkQuery(id, link));
        if (!linkResult.IsSuccess) return ReturnResult(linkResult);

        var orderResult = await Sender.Send(new CreateSecKillOrderCommand(id, time));
        if (!orderResult.IsSuccess) return ReturnResult(orderResult);

        multiThreadingCreateOrder.CreateOrder();
        return Ok();
    }

    [HttpGet("queue/status")]
    [Authorize]
    public async Task<IActionResult> GetSecKillQueue()
    {
        var result = await Sender.Send(new GetSecKillQueueQuery());
        if (result.Status == ResultStatus.NotFound) return ReturnResult(result);
        var secKillQueue = result.Value!;
        return Ok(new { status = secKillQueue.Status, orderId = secKillQueue.OrderId.ToString() });
    }
    
    [HttpGet("order/{userId:long}")]
    public async Task<IActionResult> GetSecKillOrder(long userId)
    {
        var result = await Sender.Send(new GetSecKillOrderQuery(userId));
        return ReturnResult(result);
    }
}