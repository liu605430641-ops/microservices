using DotNetCore.CAP;
using MediatR;
using Zhaoxi.MSACommerce.OrderService.Core.Enums;
using Zhaoxi.MSACommerce.OrderService.UseCases.Commands;
using Zhaoxi.MSACommerce.OrderService.UseCases.Queries;
using Zhaoxi.MSACommerce.SharedEvent.Orders;

namespace Zhaoxi.MSACommerce.OrderService.UseCases.CapSubscribes;

public class OrderSubscriber(ISender sender, ICapPublisher capPublisher) : IOrderSubscriber, ICapSubscribe
{
    [CapSubscribe(nameof(OrderCreatedEventResult), Group = nameof(OrderService))]
    public Task OrderCreatedResultReceive(OrderCreatedEventResult? result)
    {
        if(result is null) return Task.CompletedTask;
        
        foreach (var failSku in result.ResvFailSkus)
        {
            Console.WriteLine($"库存不足，商品：{failSku.SkuId}，数量：{failSku.Quantity}");
        }
        return Task.CompletedTask;
    }
    
    [CapSubscribe(nameof(OrderPayedEvent), Group = nameof(OrderService))]
    public async Task OrderPayedReceive(OrderPayedEvent @event)
    {
        await sender.Send(new UpdateOrderStatusCommand(@event.OrderId, OrderStatus.Payed));
    }
    
    [CapSubscribe(nameof(OrderTimeoutEvent), Group = nameof(OrderService))]
    public async Task OrderTimeoutReceive(OrderTimeoutEvent @event)
    {
        var result = await sender.Send(new GetOrderStatusQuery(@event.OrderId));

        if (!result.IsSuccess || result.Value is null) return;

        if (result.Value.Status == OrderStatus.UnPayed)
        {
            await sender.Send(new UpdateOrderStatusCommand(@event.OrderId, OrderStatus.Canceled));
        }
    }
}