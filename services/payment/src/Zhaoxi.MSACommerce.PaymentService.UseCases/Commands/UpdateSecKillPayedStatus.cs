using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Enmus;
using Zhaoxi.MSACommerce.PaymentService.Infrastructure.Data;
using Zhaoxi.MSACommerce.SharedEvent.Orders;
using Zhaoxi.MSACommerce.SharedEvent.SecKills;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.Commands;

public record UpdateSecKillPayedStatusCommand(long Id) : ICommand<Result>;

public class UpdateSecKillPayedStatusCommandHandler(PaymentDbContext dbContext, ICapPublisher capPublisher) : ICommandHandler<UpdateSecKillPayedStatusCommand, Result>
{
    public async Task<Result> Handle(UpdateSecKillPayedStatusCommand request, CancellationToken cancellationToken)
    {
        var payLog = await dbContext.PayLogs.FirstOrDefaultAsync(p => p.Id == request.Id,
            cancellationToken: cancellationToken);
        
        if (payLog == null) return Result.NotFound();
        
        payLog.Status = PayStatus.Payed;

        await using var trans = await dbContext.Database.BeginTransactionAsync(capPublisher, cancellationToken: cancellationToken);
        payLog.PayTime = DateTime.Now;
            
        await dbContext.SaveChangesAsync(cancellationToken);

        var secKillPayedEvent = new SecKillPayedEvent()
        {
            UserId = payLog.UserId
        };
            
        await capPublisher.PublishAsync(nameof(SecKillPayedEvent), secKillPayedEvent, cancellationToken: cancellationToken);

        await trans.CommitAsync(cancellationToken);
        
        return Result.Success();
    }
}