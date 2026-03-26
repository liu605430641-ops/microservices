using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Enmus;
using Zhaoxi.MSACommerce.PaymentService.Infrastructure.Data;
using Zhaoxi.MSACommerce.SharedEvent.Orders;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.Commands;

public record UpdateCancelPayStatusCommand(long OrderId) : ICommand<Result>;

public class UpdateCancelPayStatusCommandHandler(PaymentDbContext dbContext, ICapPublisher capPublisher) : ICommandHandler<UpdateCancelPayStatusCommand, Result>
{
    public async Task<Result> Handle(UpdateCancelPayStatusCommand request, CancellationToken cancellationToken)
    {
        var payLog = await dbContext.PayLogs.FirstOrDefaultAsync(p => p.OrderId == request.OrderId,
            cancellationToken: cancellationToken);
        
        if (payLog == null) return Result.NotFound();
        
        payLog.Status = PayStatus.Cancel;

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}