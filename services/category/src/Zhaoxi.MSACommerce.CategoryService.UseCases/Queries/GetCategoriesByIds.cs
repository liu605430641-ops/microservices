using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Zhaoxi.MSACommerce.CategoryService.UseCases.Queries;

/// <summary>
/// 获取分类及其子分类
/// </summary>
/// <param name="ParentId"></param>
public record GetCategoriesByIdsQuery(long[] ids) : IQuery<Result<List<CategoryDto>>>;

public class GetCategoriesByIdsQueryValidator : AbstractValidator<GetCategoriesByIdsQuery>
{
    public GetCategoriesByIdsQueryValidator()
    {
        RuleFor(query => query.ids)
            .Must(ids => ids.Length > 0).WithMessage("ids不能为空");
    }
}

public class GetCategoriesByIdsQueryHandler(CategoryDbContext dbContext, IFusionCache cache, IMapper mapper)
    : IQueryHandler<GetCategoriesByIdsQuery, Result<List<CategoryDto>>>
{
    public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesByIdsQuery request,
        CancellationToken cancellationToken)
    {
        // 从缓存中获取所有品类
        var allCategories = await cache.GetOrSetAsync<List<Category>>($"{nameof(Category)}", 
            async token => 
                await dbContext.Category.AsNoTracking().ToListAsync(token), 
            options => options.SetDurationInfinite(),
            token: cancellationToken);
        
        // 查询
        var categoryDtos = allCategories
            .Where(c => request.ids.Contains(c.Id))
            .Select(c => new CategoryDto(c.Id, c.Name)).ToList();
       
        return Result.Success(categoryDtos);
    }
}