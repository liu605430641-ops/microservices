using Zhaoxi.MSACommerce.SharedEvent.Orders;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.CapSubscribes;

public interface IOrderSubscriber
{
    Task OrderCanceledReceive(OrderCreatedEvent @event);
}