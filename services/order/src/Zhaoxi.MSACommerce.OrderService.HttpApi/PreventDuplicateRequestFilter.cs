using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Zhaoxi.MSACommerce.OrderService.HttpApi;

public class PreventDuplicateRequestFilter : ActionFilterAttribute
{
    private static readonly Dictionary<string,DateTime> RequestTimestamps = new();

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userId     = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var requestKey = $"{userId}:{context.HttpContext.Request.Headers["X-Request-ID"]}";

        if (RequestTimestamps.ContainsKey(requestKey))
        {
            context.Result = new BadRequestObjectResult("请勿重复提交");
            return;
        }

        RequestTimestamps[requestKey] = DateTime.UtcNow;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var userId     = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var requestKey = $"{userId}:{context.HttpContext.Request.Headers["X-Request-ID"]}";
        RequestTimestamps.Remove(requestKey);
    }
}