using Newtonsoft.Json;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.SeckillService.Core.Entities;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases.Queries;

public record GetSecKillProductByIdQuery(string Time,long Id) : IQuery<Result<SecKillProduct>>;

public class GetSecKillProductByIdQueryValidator : AbstractValidator<GetSecKillProductByIdQuery>
{
    public GetSecKillProductByIdQueryValidator()
    {
        RuleFor(query => query.Time)
            .NotEmpty();
    }
}

public class GetSecKillProductByIdQueryQueryHandler(IConnectionMultiplexer redis) : IQueryHandler<GetSecKillProductByIdQuery, Result<SecKillProduct>>
{
    public async Task<Result<SecKillProduct>> Handle(GetSecKillProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var db = redis.GetDatabase();
        var secKillValue = await db.HashGetAsync($"{RedisKeyConstants.SeckillDatePrefix}{request.Time}", request.Id);
        if(secKillValue.HasValue == false || secKillValue.IsNull) return Result.NotFound();
        var secKill = JsonConvert.DeserializeObject<SecKillProduct>(secKillValue!);

        return Result.Success(secKill!);
    }
}