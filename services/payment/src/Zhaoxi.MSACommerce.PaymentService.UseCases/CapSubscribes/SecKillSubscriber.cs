using DotNetCore.CAP;
using MediatR;
using Zhaoxi.MSACommerce.SharedEvent.SecKills;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.CapSubscribes;

public class SecKillSubscriber(ISender sender) : ISecKillSubscriber, ICapSubscribe
{
    [CapSubscribe(nameof(SecKillTimeoutEvent), Group = nameof(PaymentService))]
    public Task SecKillTimeoutReceive(SecKillTimeoutEvent @event)
    {
        // TODO 关闭支付
        Console.WriteLine($"秒杀超时{@event.UserId}-${@event.OrderId}");
        return Task.CompletedTask;
    }
}