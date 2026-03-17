using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.SharedKernel.Result;
using IResult = Zhaoxi.MSACommerce.SharedKernel.Result.IResult;

namespace Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;

public abstract class ApiControllerBase : ControllerBase
{
    protected ISender Sender => HttpContext.RequestServices.GetRequiredService<ISender>();

    [NonAction]
    protected IActionResult ReturnResult(IResult result)
    {
        switch (result.Status)
        {
            case ResultStatus.Ok:
            {
                var value = result.GetValue();
                return value is null ? NoContent() : Ok(value);
            }
            case ResultStatus.Error:
                return result.Errors is null ? BadRequest() : BadRequest(new { errors = result.Errors });

            case ResultStatus.NotFound:
                return result.Errors is null ? NotFound() : NotFound(new { errors = result.Errors });

            case ResultStatus.Invalid:
                return result.Errors is null ? BadRequest() : BadRequest(new { errors = result.Errors });

            case ResultStatus.Forbidden:
                return StatusCode(403);

            case ResultStatus.Unauthorized:
                return Unauthorized();

            default:
                return BadRequest(new { errors = result.Errors });
        }
    }
}
