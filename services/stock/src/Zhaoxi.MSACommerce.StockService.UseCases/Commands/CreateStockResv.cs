using Microsoft.EntityFrameworkCore;

namespace Zhaoxi.MSACommerce.StockService.UseCases.Commands;

public record CreateStockResvCommand(long SkuId, long OrderId, int Quantity) : ICommand<Result>;

public class CreateStockResvCommandValidator : AbstractValidator<CreateStockResvCommand>
{
    public CreateStockResvCommandValidator()
    {
        RuleFor(query => query.Quantity)
            .GreaterThan(0).WithMessage("数量必须大于0");
    }
}

public class CreateStockResvCommandHandler(StockDbContext dbContext) : ICommandHandler<CreateStockResvCommand, Result>
{
    public async Task<Result> Handle(CreateStockResvCommand request, CancellationToken cancellationToken)
    {
        var stock = await dbContext.SkuStocks.FirstOrDefaultAsync(s => s.Id == request.SkuId, cancellationToken: cancellationToken);

        if (stock is null) return Result.NotFound();

        if (stock.AvailQty < request.Quantity)  return Result.Failure("库存不足");
        
        stock.AddResvQty(request.OrderId, request.Quantity, 30);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}