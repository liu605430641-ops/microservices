using Zhaoxi.MSACommerce.SharedEvent.Orders;

namespace Zhaoxi.MSACommerce.OrderService.UseCases.CapSubscribes;

public interface IOrderSubscriber
{
    Task OrderCreatedResultReceive(OrderCreatedEventResult result);
}