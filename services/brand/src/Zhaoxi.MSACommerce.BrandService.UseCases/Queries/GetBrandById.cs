using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Zhaoxi.MSACommerce.BrandService.UseCases.Queries;

/// <summary>
/// 获取品牌
/// </summary>
/// <param name="Id"></param>
public record GetBrandByIdQuery(long Id) : IQuery<Result<BrandDto>>;

public class GetBrandByIdQueryValidator : AbstractValidator<GetBrandByIdQuery>
{
    public GetBrandByIdQueryValidator()
    {
        RuleFor(query => query.Id)
           .GreaterThan(0);
    }
}

public class GetBrandByIdQueryHandler(BrandDbContext dbContext, IFusionCache cache, IMapper mapper)
    : IQueryHandler<GetBrandByIdQuery, Result<BrandDto>>
{
    public async Task<Result<BrandDto>> Handle(GetBrandByIdQuery request,
                                               CancellationToken cancellationToken)
    {
        // 从缓存中获取品牌
        var key = $"{nameof(Brand)}:{request.Id}";
        var brandDto = await cache.GetOrSetAsync<BrandDto?>(key,
                                                            async token =>
                                                            {
                                                                var brand = await dbContext.Brands.AsNoTracking()
                                                                                           .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken: token);
                
                                                                return brand is null ? null : mapper.Map<BrandDto>(brand);
                                                            },
                                                            token: cancellationToken);

        return brandDto is null ? Result.NotFound() : Result.Success(brandDto);
    }
}