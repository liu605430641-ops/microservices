using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Zhaoxi.MSACommerce.OrderService.HttpApi;

/// <summary>
/// 这里放redis实现的版本，暂时不使用
/// </summary>
public class PreventDuplicateRequestFilter : ActionFilterAttribute
{
    // 这里使用一个静态字典来记录用户的请求时间戳，实际生产环境中应该使用分布式缓存（如 Redis）来存储这些信息，以支持多实例部署
    //如果多节点这里就会出问题
    private static readonly Dictionary<string, DateTime> RequestTimestamps = new();
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //加了一个请求id，防止同一用户在短时间内发起多个请求 所以一个用户不可能在短时间内发起多个请求，如果发起了多个请求，就认为是重复提交了
        var requestKey = $"{userId}:{context.HttpContext.Request.Headers["X-Request-ID"]}";
        if (RequestTimestamps.ContainsKey(requestKey))
        {
            context.Result = new BadRequestObjectResult("请勿重复提交");
            return;
        }

        RequestTimestamps[requestKey] = DateTime.UtcNow;
    }

    /// <summary>
    /// 过滤器执行完成后，移除请求记录，允许用户再次发起请求
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var requestKey = $"{userId}:{context.HttpContext.Request.Headers["X-Request-ID"]}";
        RequestTimestamps.Remove(requestKey);
    }
}