using DotNetCore.CAP;
using MediatR;
using Zhaoxi.MSACommerce.SharedEvent.Orders;
using Zhaoxi.MSACommerce.StockService.UseCases.Commands;

namespace Zhaoxi.MSACommerce.StockService.UseCases.CapSubscribes;

public class OrderSubscriber(ISender sender) : IOrderSubscriber, ICapSubscribe
{
    [CapSubscribe(nameof(OrderCreatedEvent), Group = nameof(StockService))]
    public async Task<OrderCreatedEventResult> OrderCreatedReceive(OrderCreatedEvent @event)
    {
        var resvFailSkus = new List<OrderSku>();
        foreach (var orderSku in @event.Skus)
        {
            var result = await sender.Send(new CreateStockResvCommand(orderSku.SkuId, @event.OrderId, orderSku.Quantity));

            if (!result.IsSuccess) resvFailSkus.Add(orderSku);
        }

        return new OrderCreatedEventResult
               {
                   OrderId      = @event.OrderId,
                   ResvFailSkus = resvFailSkus
               };
    }
    
    [CapSubscribe(nameof(OrderPayedEvent), Group = nameof(StockService))]
    public async Task OrderPayedReceive(OrderPayedEvent @event)
    {
        await sender.Send(new ApplyStockResvCommand(@event.OrderId));
    }
}