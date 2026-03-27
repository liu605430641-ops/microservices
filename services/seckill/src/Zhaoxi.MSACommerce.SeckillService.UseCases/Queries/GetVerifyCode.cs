using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases.Queries;

public record GetVerifyCodeQuery(string Code) : IQuery<Result>;

public class GetVerifyCodeQueryHandler(IConnectionMultiplexer redis, IUser user) : IQueryHandler<GetVerifyCodeQuery, Result>
{
    public async Task<Result> Handle(GetVerifyCodeQuery request, CancellationToken cancellationToken)
    {
        var db = redis.GetDatabase();
        var code = await db.StringGetDeleteAsync($"{RedisKeyConstants.SecKillVerifyCodePrefix}{user.Id}");
        if (code.IsNullOrEmpty) return Result.Failure("验证码已过期");
        return code != request.Code ? Result.Failure("验证码错误") : Result.Success();
    }
}