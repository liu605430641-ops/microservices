using Newtonsoft.Json;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.SeckillService.Core.Entities;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases.Queries;

public record GetSecKillQueueQuery : IQuery<Result<SecKillQueue>>;

public class GetSecKillQueueQueryHandler(IConnectionMultiplexer redis, IUser user)
    : IQueryHandler<GetSecKillQueueQuery, Result<SecKillQueue>>
{
    public async Task<Result<SecKillQueue>> Handle(GetSecKillQueueQuery request, CancellationToken cancellationToken)
    {
        var db = redis.GetDatabase();
        var secKillQueueValue = await db.HashGetAsync($"{RedisKeyConstants.SecKillQueueStatus}", user.Id);
        if (secKillQueueValue.IsNullOrEmpty) return Result.NotFound();
        
        var secKillQueue = JsonConvert.DeserializeObject<SecKillQueue>(secKillQueueValue);
        return secKillQueue != null ? Result.Success(secKillQueue) : Result.NotFound();
    }
}