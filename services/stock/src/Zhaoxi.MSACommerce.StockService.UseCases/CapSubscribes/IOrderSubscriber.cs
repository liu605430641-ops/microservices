using Zhaoxi.MSACommerce.SharedEvent.Orders;

namespace Zhaoxi.MSACommerce.StockService.UseCases.CapSubscribes;

public interface IOrderSubscriber
{
    Task<OrderCreatedEventResult> OrderCreatedReceive(OrderCreatedEvent orderCreatedEvent);
}