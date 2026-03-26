using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Enmus;
using Zhaoxi.MSACommerce.PaymentService.Infrastructure.Data;
using Zhaoxi.MSACommerce.SharedEvent.Orders;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.Commands;

public record UpdatePayedStatusCommand(long Id) : ICommand<Result>;

public class UpdatePayedStatusCommandHandler(PaymentDbContext dbContext, ICapPublisher capPublisher) : ICommandHandler<UpdatePayedStatusCommand, Result>
{
    public async Task<Result> Handle(UpdatePayedStatusCommand request, CancellationToken cancellationToken)
    {
        var payLog = await dbContext.PayLogs.FirstOrDefaultAsync(p => p.Id == request.Id,
            cancellationToken: cancellationToken);
        
        if (payLog == null) return Result.NotFound();
        
        payLog.Status = PayStatus.Payed;

        await using var trans = await dbContext.Database.BeginTransactionAsync(capPublisher, cancellationToken: cancellationToken);
        payLog.PayTime = DateTime.Now;
            
        await dbContext.SaveChangesAsync(cancellationToken);

        var orderPayedEvent = new OrderCreatedEvent()
        {
            OrderId = payLog.OrderId
        };
            
        await capPublisher.PublishAsync(nameof(OrderCreatedEvent), orderPayedEvent, cancellationToken: cancellationToken);

        await trans.CommitAsync(cancellationToken);
        
        return Result.Success();
    }
}