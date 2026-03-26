using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.SharedKernel.Paging;

namespace Zhaoxi.MSACommerce.ProductService.UseCases.Queries;

/// <summary>
/// 获取SPU详情
/// </summary>
/// <param name="Id"></param>
public record GetSpuFullListQuery(Pagination Pagination) : IQuery<Result<PagedList<SpuDto>>>;

public class GetSpuFullListQueryHandler(ProductDbContext dbContext, IMapper mapper)
    : IQueryHandler<GetSpuFullListQuery, Result<PagedList<SpuDto>>>
{
    public async Task<Result<PagedList<SpuDto>>> Handle(GetSpuFullListQuery request, CancellationToken cancellationToken)
    {
        var queryable = dbContext.Spus.AsNoTracking()
            .Include(x => x.Detail)
            .Include(x => x.Skus)
            .OrderBy(x => x.Id);
        var count = queryable.Count();

        if (count == 0) return Result.NotFound();
        
        var spus = await queryable
            .Skip((request.Pagination.PageNumber - 1) * request.Pagination.PageSize)
            .Take(request.Pagination.PageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        var spusDto = mapper.Map<List<SpuDto>>(spus);
        var pagedList = new PagedList<SpuDto>(spusDto, count, request.Pagination);
        return Result.Success(pagedList);
    }
}