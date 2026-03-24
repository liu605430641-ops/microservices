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
    // 1️⃣ 查询总数
    var totalCount = await dbContext.Spus.AsNoTracking().CountAsync(cancellationToken);
    if (totalCount == 0) 
        return Result.NotFound();

    // 2️⃣ 分页查询 SPU 基础数据（只查询需要的字段即可提高性能）
    var spus = await dbContext.Spus.AsNoTracking()
        .OrderBy(x => x.Id)
        .Skip((request.Pagination.PageNumber - 1) * request.Pagination.PageSize)
        .Take(request.Pagination.PageSize)
        .ToListAsync(cancellationToken);

    // 3️⃣ 查询关联 Detail 和 Skus
    var spuIds = spus.Select(s => s.Id).ToList();

    var details = await dbContext.SpuDetails
        .AsNoTracking()
        .Where(d => spuIds.Contains(d.Id))
        .ToListAsync(cancellationToken);

    var skus = await dbContext.Skus
        .AsNoTracking()
        .Where(s => spuIds.Contains(s.SpuId))
        .ToListAsync(cancellationToken);

    // 4️⃣ 手动映射到 DTO
    var spusDto = spus.Select(spu =>
    {
        var dto = mapper.Map<SpuDto>(spu); // 映射 SPU 基础字段

        // 手动映射 Detail
        var detailEntity = details.FirstOrDefault(d => d.Id == spu.Id);
        dto.Detail = detailEntity != null
            ? new SpuDetailDto
            {
            
                Introduction = detailEntity.Introduction,
                Parameter = detailEntity.Parameter,
                Spec = detailEntity.Spec,
              
            }
            : null;

        // 手动映射 Skus
        dto.Skus = skus
            .Where(s => s.SpuId == spu.Id)
            .Select(s => new SkuDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
             
            })
            .ToList();

        return dto;
    }).ToList();

    // 5️⃣ 构建分页结果
    var pagedList = new PagedList<SpuDto>(spusDto, totalCount, request.Pagination);

    return Result.Success(pagedList);
}
}