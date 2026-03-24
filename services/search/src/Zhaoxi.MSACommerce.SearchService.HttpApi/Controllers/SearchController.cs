using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.SearchService.UseCases.Queries;
using Zhaoxi.MSACommerce.SharedKernel.Paging;

namespace Zhaoxi.MSACommerce.SearchService.HttpApi.Controllers;

[Route("api/search")]
[ApiController]
public class SearchController: ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetById(string keyword, [FromQuery]Pagination pagination)
    {
        var result = await Sender.Send(new GetProductQuery(keyword, pagination));
        return ReturnResult(result);
    }
}
