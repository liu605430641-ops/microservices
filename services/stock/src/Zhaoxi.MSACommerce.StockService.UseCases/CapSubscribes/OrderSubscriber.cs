using DotNetCore.CAP;
using MediatR;
using Zhaoxi.MSACommerce.SharedEvent.Orders;
using Zhaoxi.MSACommerce.StockService.UseCases.Commands;

namespace Zhaoxi.MSACommerce.StockService.UseCases.CapSubscribes;

public class OrderSubscriber(ISender sender) : IOrderSubscriber,ICapSubscribe
{
    /// <summary>
    /// 订阅  OrderCreatedEvent 事件，订单创建后，创建一个 CreateStockResvCommand 命令，预占库存
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(OrderCreatedEvent),Group = nameof(StockService))]
    public async Task<OrderCreatedEventResult> OrderCreatedReceive(OrderCreatedEvent @event)
    {
        var resvFailSkus = new List<OrderSku>();

        foreach (var orderSku in @event.Skus)
        {
            var result = await sender.Send(new CreateStockResvCommand(orderSku.SkuId,@event.OrderId,orderSku.Quantity));

            if (!result.IsSuccess) resvFailSkus.Add(orderSku);
        }

        return new OrderCreatedEventResult { OrderId = @event.OrderId,ResvFailSkus = resvFailSkus };
    }

    /// <summary>
    /// 订阅  OrderPayedEvent 事件，订单支付成功后，创建一个 ApplyStockResvCommand 命令，申请锁定库存
    /// </summary>
    /// <param name="event"></param>
    [CapSubscribe(nameof(OrderPayedEvent),Group = nameof(StockService))]
    public async Task OrderPayedReceive(OrderPayedEvent @event)
    {
        await sender.Send(new ApplyStockResvCommand(@event.OrderId));
    }

    //订单取消事件，订单取消后，创建一个 CancelStockResvCommand 命令，取消预占库存
    [CapSubscribe(nameof(OrderCanceledEvent),Group = nameof(StockService))]
    public async Task OrderCanceledReceive(OrderCanceledEvent @event)
    {
        await sender.Send(new UpdateStockResvCommand(@event.OrderId,StockResvStatus.Release));
    }
}