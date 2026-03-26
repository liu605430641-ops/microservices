using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.OrderService.UseCases;
using Zhaoxi.MSACommerce.OrderService.UseCases.Commands;
using Zhaoxi.MSACommerce.OrderService.UseCases.Queries;

namespace Zhaoxi.MSACommerce.OrderService.HttpApi.Controllers;

[Route("api/order")]
[ApiController]
public class OrderController : ApiControllerBase
{
    [HttpPost]
    [Authorize]
    [PreventDuplicateRequestFilter]
    public async Task<IActionResult> Create(OrderForCreateDto createDto)
    {
        var result = await Sender.Send(new CreateOrderCommand(createDto));

        return result.IsSuccess ? Ok(new { orderId = result.Value.ToString() }) : ReturnResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get(long id)
    {
        var result = await Sender.Send(new GetOrderQuery(id));
        return ReturnResult(result);
    }
}