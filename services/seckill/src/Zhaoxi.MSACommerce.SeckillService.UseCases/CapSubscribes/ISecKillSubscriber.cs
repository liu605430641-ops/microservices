using Zhaoxi.MSACommerce.SharedEvent.SecKills;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases.CapSubscribes;

public interface ISecKillSubscriber
{
    Task SecKillPayedReceive(SecKillPayedEvent @event);
    
    Task SecKillTimeoutReceive(SecKillTimeoutEvent @event);

}