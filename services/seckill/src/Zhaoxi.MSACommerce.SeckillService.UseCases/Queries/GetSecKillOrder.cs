using Newtonsoft.Json;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.SeckillService.Core.Entities;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases.Queries;

public record GetSecKillOrderQuery(long UserId) : IQuery<Result<SeckillOrder>>;

public class GetSecKillOrderQueryHandler(IConnectionMultiplexer redis)
    : IQueryHandler<GetSecKillOrderQuery, Result<SeckillOrder>>
{
    public async Task<Result<SeckillOrder>> Handle(GetSecKillOrderQuery request, CancellationToken cancellationToken)
    {
        var db = redis.GetDatabase();
        var secKillOrderValue = await db.HashGetAsync($"{RedisKeyConstants.SecKillOrder}", request.UserId);
        if (secKillOrderValue.IsNullOrEmpty) return Result.NotFound();
        
        var secKillOrder = JsonConvert.DeserializeObject<SeckillOrder>(secKillOrderValue);
        return secKillOrder != null ? Result.Success(secKillOrder) : Result.NotFound();
    }
}