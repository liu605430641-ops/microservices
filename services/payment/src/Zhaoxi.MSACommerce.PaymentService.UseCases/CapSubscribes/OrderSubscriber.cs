using DotNetCore.CAP;
using MediatR;
using Zhaoxi.MSACommerce.CategoryService.Core.Enmus;
using Zhaoxi.MSACommerce.PaymentService.UseCases.Commands;
using Zhaoxi.MSACommerce.SharedEvent.Orders;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.CapSubscribes;

public class OrderSubscriber(ISender sender) : IOrderSubscriber, ICapSubscribe
{
    [CapSubscribe(nameof(OrderCanceledEvent), Group = nameof(PaymentService))]
    public async Task OrderCanceledReceive(OrderCreatedEvent @event)
    {
        await sender.Send(new UpdateCancelPayStatusCommand(@event.OrderId));
    }
}