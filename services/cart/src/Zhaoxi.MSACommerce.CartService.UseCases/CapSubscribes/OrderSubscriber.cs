using DotNetCore.CAP;
using MediatR;
using Zhaoxi.MSACommerce.CartService.UseCases.Commands;
using Zhaoxi.MSACommerce.SharedEvent.Orders;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.CartService.UseCases.CapSubscribes;

public class OrderSubscriber(ISender sender, IUser user) : IOrderSubscriber, ICapSubscribe
{
    [CapSubscribe(nameof(OrderCreatedEvent), Group = nameof(CartService))]
    public async Task OrderCreatedReceive(OrderCreatedEvent @event)
    {
        foreach (var sku in @event.Skus)
        {
            await sender.Send(new DeleteItemCommand(user.Id, sku.SkuId));
        }
    }
}