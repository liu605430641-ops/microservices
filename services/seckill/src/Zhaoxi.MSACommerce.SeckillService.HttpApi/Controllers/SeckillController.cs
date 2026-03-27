using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.SeckillService.Infrastructure;
using Zhaoxi.MSACommerce.SeckillService.UseCases.Queries;

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
    
    [HttpGet("{date}/{id}")]
    public async Task<IActionResult> GetSecKillById(string date, string id)
    {
        var result = await Sender.Send(new GetSecKillProductByIdQuery(date, id));
        return ReturnResult(result);
    }
    
    [HttpGet("list/{time}")]
    public async Task<IActionResult> GetSecKillProductsByTime(string time)
    {
        var result = await Sender.Send(new GetSecKillProductsByTimeQuery(time));
        return ReturnResult(result);
    }
}
