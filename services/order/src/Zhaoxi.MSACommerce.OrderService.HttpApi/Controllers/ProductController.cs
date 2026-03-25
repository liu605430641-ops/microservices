using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.OrderService.UseCases;
using Zhaoxi.MSACommerce.OrderService.UseCases.Commands;

namespace Zhaoxi.MSACommerce.OrderService.HttpApi.Controllers;

[Route("api/order")]
[ApiController]
[Authorize]
public class ProductController : ApiControllerBase
{
    [HttpPost]
    [PreventDuplicateRequestFilter]
    public async Task<IActionResult> CreateOrder(OrderForCreateDto createDto)
    {
        var result = await Sender.Send(new CreateOrderCommand(createDto));
        return ReturnResult(result);
    }
}
