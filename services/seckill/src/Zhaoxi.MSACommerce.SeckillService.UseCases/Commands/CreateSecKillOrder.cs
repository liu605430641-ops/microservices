using Newtonsoft.Json;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.SeckillService.Core.Entities;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases.Commands;

public record CreateSecKillOrderCommand(long Id, string Time) : ICommand<Result>;

public class CreateSecKillOrderCommandHandler(IConnectionMultiplexer redis, IUser user, MultiThreadingCreateOrder multiThreadingCreateOrder) : ICommandHandler<CreateSecKillOrderCommand, Result>
{
    public async Task<Result> Handle(CreateSecKillOrderCommand request, CancellationToken cancellationToken)
    {
        var db = redis.GetDatabase();
        
        //递增，判断是否排队
        var userQueueCount = await db.HashIncrementAsync($"{RedisKeyConstants.SecKillUserQueueCountPrefix}{request.Id}", user.Id);
        if (userQueueCount > 1) return Result.Failure("不允许重复抢单");
        
        // 检查秒杀商品库存
        var num = await db.StringGetAsync($"{RedisKeyConstants.SecKillStockPrefix}{request.Id}");
        if (num.IsNullOrEmpty) return Result.NotFound("秒杀商品不存在");
        if (Convert.ToInt32(num) <= 0) return Result.Failure("秒杀商品已售罄");
        
        // 排队信息
        var seckillQueue = new SecKillQueue
        {
            UserId = user.Id,
            Username = user.Username,
            SecKillId = request.Id,
            Time = request.Time
        };
        var seckillQueueValue = JsonConvert.SerializeObject(seckillQueue);
        
        // 将秒杀抢单信息存入到Redis中,这里采用List方式存储,List本身是一个队列
        await db.ListLeftPushAsync(RedisKeyConstants.SecKillQueueList, seckillQueueValue); 
        // 设置排队状态标识
        await db.HashSetAsync(RedisKeyConstants.SecKillQueueStatus, user.Id, seckillQueueValue);
        multiThreadingCreateOrder.CreateOrder();
        return Result.Success();
    }
}