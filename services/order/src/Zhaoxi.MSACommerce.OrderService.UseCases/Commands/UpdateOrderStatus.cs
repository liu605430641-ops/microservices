using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.OrderService.Core.Enums;
using Zhaoxi.MSACommerce.SharedEvent.Orders;

namespace Zhaoxi.MSACommerce.OrderService.UseCases.Commands;

public record UpdateOrderStatusCommand(long OrderId,OrderStatus Status) : ICommand<Result>;

public class UpdateOrderStatusHandler(OrderDbContext dbContext,ICapPublisher capPublisher) : ICommandHandler<UpdateOrderStatusCommand,Result>
{
    public async Task<Result> Handle(UpdateOrderStatusCommand request,CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
                                   .Include(o => o.OrderInfo)
                                   .Where(o => o.Id == request.OrderId)
                                   .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (order == null) return Result.NotFound();

        order.OrderInfo.Status = request.Status;

        switch (request.Status)
        {
            case OrderStatus.Canceled:
            {
                order.OrderInfo.CloseTime = DateTime.Now;
                await using var trans = await dbContext.Database.BeginTransactionAsync(capPublisher,cancellationToken: cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                var orderCanceledEvent = new OrderCanceledEvent { OrderId = order.Id };

                await capPublisher.PublishAsync(nameof(OrderCanceledEvent),orderCanceledEvent,cancellationToken: cancellationToken);
                await trans.CommitAsync(cancellationToken);
                break;
            }
            case OrderStatus.Payed:
            {
                order.OrderInfo.PaymentTime = DateTime.Now;
                await dbContext.SaveChangesAsync(cancellationToken);
                break;
            }
            default:
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                break;
            }
        }

        return Result.Success();
    }
}