using DotNetCore.CAP;
using MediatR;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.SeckillService.Core.Enums;
using Zhaoxi.MSACommerce.SeckillService.Infrastructure.Data;
using Zhaoxi.MSACommerce.SeckillService.UseCases.Queries;
using Zhaoxi.MSACommerce.SharedEvent.SecKills;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases.CapSubscribes;

public class SecKillSubscriber(ISender sender, ICapPublisher capPublisher, SecKillDbContext dbContext, IConnectionMultiplexer redis) : ISecKillSubscriber, ICapSubscribe
{

    [CapSubscribe(nameof(SecKillPayedEvent), Group = nameof(SeckillService))]
    public async Task SecKillPayedReceive(SecKillPayedEvent @event)
    {
        var result = await sender.Send(new GetSecKillOrderQuery(@event.UserId));
        if (result.IsSuccess)
        {
            var order = result.Value;
            order.Status = OrderStatus.Payed;
            order.PayTime = DateTime.Now;
            dbContext.SeckillOrders.Add(order);
            await dbContext.SaveChangesAsync();
        }

        var db = redis.GetDatabase();
        await db.HashDeleteAsync(RedisKeyConstants.SecKillOrder, @event.UserId);
        await db.HashDeleteAsync(RedisKeyConstants.SecKillQueueStatus, @event.UserId);
    }

    [CapSubscribe(nameof(SecKillTimeoutEvent), Group = nameof(SeckillService))]
    public async Task SecKillTimeoutReceive(SecKillTimeoutEvent @event)
    {
        // TODO 
        // 通过用户ID在 Redis 查出订单
        // 如果能查出来==》用户未支付
        // 如果查不出来==》用户已支付
        // 查MySQL==》如果存在==》已支付==》否则未支付
        
        // 库存回滚(Redis、MySQL)
        // Redis：三个地方要回滚
        // 判断库存是否为0 ==》还要去MySQL回滚
        // 清理用户抢单排队信息
        
        var db = redis.GetDatabase();
        await db.HashDeleteAsync(RedisKeyConstants.SecKillOrder, @event.UserId);
        await db.HashDeleteAsync(RedisKeyConstants.SecKillQueueStatus, @event.UserId);

        Console.WriteLine($"秒杀超时{@event.UserId}-${@event.OrderId}");
    }
    
}