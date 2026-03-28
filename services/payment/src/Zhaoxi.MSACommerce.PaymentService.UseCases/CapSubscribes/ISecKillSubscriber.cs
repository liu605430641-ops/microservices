using Zhaoxi.MSACommerce.SharedEvent.SecKills;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.CapSubscribes;

public interface ISecKillSubscriber
{
    Task SecKillTimeoutReceive(SecKillTimeoutEvent @event);
}