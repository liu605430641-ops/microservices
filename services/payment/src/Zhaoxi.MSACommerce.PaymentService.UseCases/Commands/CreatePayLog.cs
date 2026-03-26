using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Enmus;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.PaymentService.Infrastructure.Data;
using Zhaoxi.MSACommerce.PaymentService.UseCases.Apis;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.Commands;

public record CreatePayLogCommand(long OrderId) : ICommand<Result<long>>;

public class CreatePayLogCommandHandler(PaymentDbContext dbContext, 
    IServiceClient<IOrderServiceApi> orderService, 
    IUser user) : ICommandHandler<CreatePayLogCommand, Result<long>>
{
    public async Task<Result<long>> Handle(CreatePayLogCommand request, CancellationToken cancellationToken)
    {
        var payLog = await dbContext.PayLogs.FirstOrDefaultAsync(x => x.OrderId == request.OrderId, cancellationToken: cancellationToken);
      
        if (payLog != null)
        {
            if (payLog.Status == PayStatus.UnPay)
            {
                return Result.Success(payLog.Id);
            }
            if (payLog.Status == PayStatus.Cancel)
            {
                return Result.Failure("订单已取消");
            }
            if(payLog.Status == PayStatus.Payed)
            {
                return Result.Failure("订单已支付");
            }
        }
        
        var response = await orderService.ServiceApi.GetOrderAsync(request.OrderId);
        
        if (!response.IsSuccessStatusCode) return Result.Failure("订单不存在");
        
        var order = response.Content!;
        
        payLog = new PayLog(request.OrderId, order.ActualPay, user.Id);
        
        dbContext.PayLogs.Add(payLog);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success(payLog.Id);
    }
}