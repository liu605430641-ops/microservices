using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Zhaoxi.MSACommerce.CategoryService.UseCases.Queries;

/// <summary>
/// 获取分类下的规格参数
/// </summary>
/// <param name="CategoryId"></param>
public record GetSpecsByCategoryQuery(long CategoryId) : IQuery<Result<List<SpecKeyDto>>>;

public class GetSpecsByCategoryQueryValidator : AbstractValidator<GetSpecsByCategoryQuery>
{
    public GetSpecsByCategoryQueryValidator()
    {
        RuleFor(query => query.CategoryId)
            .GreaterThan(0);
    }
}

public class GetSpecsByCategoryQueryHandler(CategoryDbContext dbContext, IFusionCache cache)
    : IQueryHandler<GetSpecsByCategoryQuery, Result<List<SpecKeyDto>>>
{
    public async Task<Result<List<SpecKeyDto>>> Handle(GetSpecsByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        // 从缓存中获取规则参数
        var key = $"{nameof(SpecKey)}:{request.CategoryId}";
        var specKeysDto = await cache.GetOrSetAsync<List<SpecKeyDto>>(key,
            async token =>
            {
                var specKeys = await dbContext.SpecKeys.AsNoTracking()
                    .Where(s => s.CategoryId == request.CategoryId)
                    .Select(s => new SpecKeyDto(s.Id, s.Name))
                    .ToListAsync(token);

                return specKeys;
            },
                
            options => options.SetDurationInfinite(),
            token: cancellationToken);

        if (specKeysDto.Count == 0) return Result.NotFound();
       
        return Result.Success(specKeysDto);
    }
}