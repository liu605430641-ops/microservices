using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Zhaoxi.MSACommerce.CategoryService.UseCases.Queries;

/// <summary>
/// 获取分类下的规格参数
/// </summary>
/// <param name="CategoryId"></param>
public record GetParemetersByCategoryQuery(long CategoryId) : IQuery<Result<List<ParameterGroupDto>>>;

public class GetParemetersByCategoryQueryValidator : AbstractValidator<GetParemetersByCategoryQuery>
{
    public GetParemetersByCategoryQueryValidator()
    {
        RuleFor(query => query.CategoryId)
            .GreaterThan(0);
    }
}

public class GetParemetersByCategoryQueryHandler(CategoryDbContext dbContext, IFusionCache cache)
    : IQueryHandler<GetParemetersByCategoryQuery, Result<List<ParameterGroupDto>>>
{
    public async Task<Result<List<ParameterGroupDto>>> Handle(GetParemetersByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        // 从缓存中获取参数
        var key = $"{nameof(ParameterKey)}:{request.CategoryId}";
        var parameterDto = await cache.GetOrSetAsync<List<ParameterGroupDto>?>(key,
            async token =>
            {
                var parameterKeys = await dbContext.ParameterGroups.AsNoTracking()
                    .Include(group => group.ParameterKeys)
                    .Where(group => group.CategoryId == request.CategoryId)
                    .ToListAsync(token);

                return parameterKeys.Select(sg => new ParameterGroupDto
                {
                    Id = sg.Id,
                    Name = sg.Name,
                    ParameterKeys = sg.ParameterKeys.Select(s => new ParameterKeyDto(s.Id, s.Name)).ToList()
                }).ToList();
            },
                
            options => options.SetDurationInfinite(),
            token: cancellationToken);

        if (parameterDto is null || parameterDto.Count == 0) return Result.NotFound();
       
        return Result.Success(parameterDto);
    }
}