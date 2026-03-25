using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.StockService.UseCases.Commands;

namespace Zhaoxi.MSACommerce.StockService.HttpApi.Controllers;

[Route("api/stock")]
[ApiController]
public class CategoryController() : ApiControllerBase
{
    [HttpPost("resv")]
    public async Task<IActionResult> CreateStockResv(CreateStockResvCommand request)
    {
        var result = await Sender.Send(request);
        return ReturnResult(result);
    }
}
