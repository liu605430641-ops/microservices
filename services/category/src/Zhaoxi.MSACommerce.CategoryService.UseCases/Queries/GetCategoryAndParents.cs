using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Zhaoxi.MSACommerce.CategoryService.UseCases.Queries;

/// <summary>
/// 获取分类及其所有父类
/// </summary>
/// <param name="Id"></param>
public record GetCategoryAndParentsQuery(long Id) : IQuery<Result<List<CategoryDto>>>;

public class GetCategoryAndParentsValidator : AbstractValidator<GetCategoryAndParentsQuery>
{
    public GetCategoryAndParentsValidator()
    {
        RuleFor(query => query.Id)
            .GreaterThan(0);
    }
}

public class GetCategoryAndParentsQueryHandler(CategoryDbContext dbContext, IFusionCache cache, IMapper mapper)
    : IQueryHandler<GetCategoryAndParentsQuery, Result<List<CategoryDto>>>
{
    public async Task<Result<List<CategoryDto>>> Handle(GetCategoryAndParentsQuery request,
        CancellationToken cancellationToken)
    {
        // 从缓存中获取所有品类
        var allCategories = await cache.GetOrSetAsync<List<Category>>($"{nameof(Category)}", 
            async token => 
                await dbContext.Category.AsNoTracking().ToListAsync(token), 
            options => options.SetDurationInfinite(),
            token: cancellationToken);
        
        var category = allCategories.FirstOrDefault(c => c.Id == request.Id);

        if (category is null) return Result.NotFound();
        
        
        var categoryDtos = new List<CategoryDto>();
        // 添加目标类别
        categoryDtos.Add(new CategoryDto(category.Id, category.Name));
        // 递归查找所有父类
        var categoryLookup = allCategories.ToDictionary(c => c.Id, c => c);
        var currentCategoryId = request.Id;
        while (categoryLookup.TryGetValue(currentCategoryId, out var currentCategory))
        {
            var parentCategoryId = currentCategory.ParentId;
            if (categoryLookup.TryGetValue(parentCategoryId, out var parentCategory))
            {
                categoryDtos.Insert(0, new CategoryDto(parentCategory.Id, parentCategory.Name));
                currentCategoryId = parentCategoryId;
            }
            else
            {
                break;
            }
        }
        
        return Result.Success(categoryDtos);
    }
}