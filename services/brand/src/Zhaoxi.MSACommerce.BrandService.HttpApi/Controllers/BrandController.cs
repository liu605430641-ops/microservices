using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.BrandService.UseCases.Queries;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;

namespace Zhaoxi.MSACommerce.BrandService.HttpApi.Controllers;

[Route("api/brand")]
[ApiController]
public class BrandController() : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await Sender.Send(new GetBrandByIdQuery(id));
        return ReturnResult(result);
    }
    
    [HttpGet("list")]
    public async Task<IActionResult> GetByIds(long[] ids)
    {
        var result = await Sender.Send(new GetBrandsByIdsQuery(ids));
        return ReturnResult(result);
    }
}
