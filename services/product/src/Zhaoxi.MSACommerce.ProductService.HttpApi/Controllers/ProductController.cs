using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.ProductService.UseCases.Queries;

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
}
