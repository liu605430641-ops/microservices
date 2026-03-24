using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.ProductService.UseCases.Queries;
using Zhaoxi.MSACommerce.SharedKernel.Paging;

namespace Zhaoxi.MSACommerce.ProductService.HttpApi.Controllers;

[Route("api/product")]
[ApiController]
public class ProductController() : ApiControllerBase
{
    [HttpGet("spu")]
    public async Task<IActionResult> Get(long id)
    {
        var result = await Sender.Send(new GetSpuFullQuery(id));
        return ReturnResult(result);
    }
    [HttpGet("spu/list")]
    public async Task<IActionResult> GetList([FromQuery]Pagination pagination)
    {
        var result = await Sender.Send(new GetSpuFullListQuery(pagination));
        Response.Headers.Append("Pagination", JsonConvert.SerializeObject(result.Value?.MetaData));
        return ReturnResult(result);
    }
}
