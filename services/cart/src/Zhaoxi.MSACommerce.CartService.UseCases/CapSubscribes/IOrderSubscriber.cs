using Zhaoxi.MSACommerce.SharedEvent.Orders;

namespace Zhaoxi.MSACommerce.CartService.UseCases.CapSubscribes;

public interface IOrderSubscriber
{
    Task OrderCreatedReceive(OrderCreatedEvent orderCreatedEvent);
}