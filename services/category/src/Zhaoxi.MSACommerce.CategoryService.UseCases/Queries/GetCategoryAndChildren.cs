using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Zhaoxi.MSACommerce.CategoryService.UseCases.Queries;

/// <summary>
/// 获取分类及其子分类
/// </summary>
/// <param name="ParentId"></param>
public record GetCategoryAndChildrenQuery(long ParentId) : IQuery<Result<List<CategoryDto>>>;

public class GetCategoryAndChildrenValidator : AbstractValidator<GetCategoryAndChildrenQuery>
{
    public GetCategoryAndChildrenValidator()
    {
        RuleFor(query => query.ParentId)
            .GreaterThan(0);
    }
}

public class GetCategoryAndChildrenQueryHandler(CategoryDbContext dbContext, IFusionCache cache, IMapper mapper)
    : IQueryHandler<GetCategoryAndChildrenQuery, Result<List<CategoryDto>>>
{
    public async Task<Result<List<CategoryDto>>> Handle(GetCategoryAndChildrenQuery request,
        CancellationToken cancellationToken)
    {
        // 从缓存中获取所有品类
        var allCategories = await cache.GetOrSetAsync<List<Category>>($"{nameof(Category)}", 
            async token => 
                await dbContext.Category.AsNoTracking().ToListAsync(token), 
            options => options.SetDurationInfinite(),
            token: cancellationToken);
        
        // 查询并排序
        var categoryDtos = allCategories
            .Where(c => c.ParentId == request.ParentId)
            .OrderBy(c => c.Sort)
            .Select(c => new CategoryDto(c.Id, c.Name)).ToList();
       
        return Result.Success(categoryDtos);
    }
}