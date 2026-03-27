using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases.Queries;

public record GetSecKillLinkQuery(long Id, string Link) : IQuery<Result>;

public class GetSecKillLinkQueryHandler(IConnectionMultiplexer redis, IUser user) : IQueryHandler<GetSecKillLinkQuery, Result>
{
    public async Task<Result> Handle(GetSecKillLinkQuery request, CancellationToken cancellationToken)
    {
        var db = redis.GetDatabase();
        var key = $"{RedisKeyConstants.SecKillLinkPrefix}{user.Id}";
        var link = await db.HashGetAsync(key,request.Id);
        if (link.IsNullOrEmpty) return Result.Failure("秒杀链接已失效");
        await db.HashDeleteAsync(key,request.Id);
        return link != request.Link ? Result.Failure("秒杀链接无效") : Result.Success();
    }
}