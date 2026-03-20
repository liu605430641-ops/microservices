using Microsoft.EntityFrameworkCore;

namespace Zhaoxi.MSACommerce.ProductService.UseCases.Queries;

/// <summary>
/// 获取SPU详情
/// </summary>
/// <param name="Id"></param>
public record GetSpuFullQuery(long Id) : IQuery<Result<SpuDto>>;

public class GetSpuFullQueryValidator : AbstractValidator<GetSpuFullQuery>
{
    public GetSpuFullQueryValidator()
    {
        RuleFor(query => query.Id)
            .GreaterThan(0);
    }
}

public class GetSpuFullQueryHandler(ProductDbContext dbContext, IMapper mapper)
    : IQueryHandler<GetSpuFullQuery, Result<SpuDto>>
{
    public async Task<Result<SpuDto>> Handle(GetSpuFullQuery request,
        CancellationToken cancellationToken)
    {
        var spu = await dbContext.Spus
            .Include(x => x.Detail)
            .Include(x => x.Skus)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if (spu is null) return Result.NotFound();
        
        var spuDto = mapper.Map<SpuDto>(spu);
        return Result.Success(spuDto);
    }
}