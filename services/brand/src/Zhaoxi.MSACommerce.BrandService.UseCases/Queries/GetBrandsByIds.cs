using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Zhaoxi.MSACommerce.BrandService.UseCases.Queries;

/// <summary>
/// 获取品牌
/// </summary>
/// <param name="Id"></param>
public record GetBrandsByIdsQuery(long[] Ids) : IQuery<Result<List<BrandDto>>>;

public class GetBrandsByIdsQueryValidator : AbstractValidator<GetBrandsByIdsQuery>
{
    public GetBrandsByIdsQueryValidator()
    {
        RuleFor(query => query.Ids)
            .Must(ids => ids.Length > 0).WithMessage("Ids不能为空");
    }
}

public class GetBrandsByIdsQueryHandler(BrandDbContext dbContext, IFusionCache cache, IMapper mapper)
    : IQueryHandler<GetBrandsByIdsQuery, Result<List<BrandDto>>>
{
    public async Task<Result<List<BrandDto>>> Handle(GetBrandsByIdsQuery request,
        CancellationToken cancellationToken)
    {
        var brandsDto = new List<BrandDto>();
        
        foreach (var id in request.Ids)
        {
            // 从缓存中获取品牌
            var key = $"{nameof(Brand)}:{id}";
            var brandDto = await cache.GetOrSetAsync<BrandDto?>(key,
                async token =>
                {
                    var brand = await dbContext.Brands.AsNoTracking()
                        .FirstOrDefaultAsync(b => b.Id == id, cancellationToken: token);
                
                    return brand is null ? null : mapper.Map<BrandDto>(brand);
                },
                token: cancellationToken);
            
            if (brandDto is null) continue;
            
            brandsDto.Add(brandDto);
        }
        
        return brandsDto.Count == 0 ? Result.NotFound() : Result.Success(brandsDto);
    }
}