using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.SharedKernel.Paging;

namespace Zhaoxi.MSACommerce.ProductService.UseCases.Queries;

public record GetSkuListByIdsQuery(long[] SkuIds) : IQuery<Result<List<SkuDto>>>;

public class GetSkuListByIdsQueryValidator : AbstractValidator<GetSkuListByIdsQuery>
{
    public GetSkuListByIdsQueryValidator()
    {
        RuleFor(query => query.SkuIds)
            .Must(s => s.Length > 0);
    }
}

public class GetSkuListByIdsQueryHandler(ProductDbContext dbContext, IMapper mapper)
    : IQueryHandler<GetSkuListByIdsQuery, Result<List<SkuDto>>>
{
    public async Task<Result<List<SkuDto>>> Handle(GetSkuListByIdsQuery request, CancellationToken cancellationToken)
    {
        var skus = await dbContext.Skus.AsNoTracking()
            .Where(x => request.SkuIds.Contains(x.Id))
            .OrderBy(x => x.Id).ToListAsync(cancellationToken: cancellationToken);

        if (skus.Count == 0) return Result.NotFound();
        
        var skusDto = mapper.Map<List<SkuDto>>(skus);
        return Result.Success(skusDto);
    }
}