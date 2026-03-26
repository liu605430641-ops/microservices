using Zhaoxi.MSACommerce.SharedEvent.Orders;

namespace Zhaoxi.MSACommerce.OrderService.UseCases.CapSubscribes;

public interface IOrderSubscriber
{
    Task OrderCreatedResultReceive(OrderCreatedEventResult? result);

    Task OrderPayedReceive(OrderPayedEvent @event);
    
    Task OrderTimeoutReceive(OrderTimeoutEvent @event);

}