using Microsoft.EntityFrameworkCore;

namespace Zhaoxi.MSACommerce.StockService.UseCases.Commands;

public record ApplyStockResvCommand(long OrderId) : ICommand<Result>;

public class ApplyStockResvCommandHandler(StockDbContext dbContext) : ICommandHandler<ApplyStockResvCommand,Result>
{
    public async Task<Result> Handle(ApplyStockResvCommand request,CancellationToken cancellationToken)
    {
        var resvs = await dbContext.StockResvs
                                   .Where(p => p.OrderId == request.OrderId)
                                   .ToListAsync(cancellationToken: cancellationToken);

        if (resvs.Count == 0) return Result.NotFound();

        var skuIds = resvs.Select(r => r.SkuId);

        var skuStocks = await dbContext.SkuStocks.Where(p => skuIds.Contains(p.Id))
                                       .ToListAsync(cancellationToken: cancellationToken);

        foreach (var skuStock in skuStocks)
        {
            var resv = resvs.First(r => r.SkuId == skuStock.Id);

            skuStock.ResvQty  -= resv.ResvQty;
            skuStock.TotalQty -= resv.ResvQty;
        }

        dbContext.StockResvs.RemoveRange(resvs);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}