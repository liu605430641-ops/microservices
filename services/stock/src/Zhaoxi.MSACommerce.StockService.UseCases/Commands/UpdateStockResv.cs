using Microsoft.EntityFrameworkCore;

namespace Zhaoxi.MSACommerce.StockService.UseCases.Commands;

public enum StockResvStatus
{
    Apply,
    Release
}

public record UpdateStockResvCommand(long OrderId, StockResvStatus ResvStatus) : ICommand<Result>;

public class UpdateStockResvCommandHandler(StockDbContext dbContext) : ICommandHandler<UpdateStockResvCommand, Result>
{
    public async Task<Result> Handle(UpdateStockResvCommand request, CancellationToken cancellationToken)
    {
        var resvs = await dbContext.StockResvs
            .Include(x=>x.SkuStock)
            .Where(p => p.OrderId == request.OrderId)
            .ToListAsync(cancellationToken: cancellationToken);
        
        if (resvs.Count == 0) return Result.NotFound();
        
        // var skuIds = resvs.Select(r => r.SkuId);
        // var skuStocks = await dbContext.SkuStocks.Where(p => skuIds.Contains(p.Id)).ToListAsync(cancellationToken: cancellationToken);
        
        foreach (var resv in resvs)
        {
            if (request.ResvStatus == StockResvStatus.Apply)
            {
                resv.SkuStock.ApplyResvQty(resv.ResvQty);
            }
            else
            {
                resv.SkuStock.ReleseResvQty(resv.ResvQty);
            }
        }
        
        //
        // foreach (var skuStock in skuStocks)
        // {
        //     var resv = resvs.First(r=>r.SkuId == skuStock.Id);
        //
        //     skuStock.ResvQty -= resv.ResvQty;
        //     skuStock.TotalQty -= resv.ResvQty;
        // }
        
        dbContext.StockResvs.RemoveRange(resvs);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}