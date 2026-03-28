using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Enmus;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.PaymentService.Infrastructure.Data;
using Zhaoxi.MSACommerce.PaymentService.UseCases.Apis;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.Commands;

public record CreateSecKillPayLogCommand(long OrderId) : ICommand<Result<long>>;

public class CreateSecKillPayLogCommandHandler(PaymentDbContext dbContext, 
    IServiceClient<ISeckillServiceApi> seckillService, 
    IUser user) : ICommandHandler<CreateSecKillPayLogCommand, Result<long>>
{
    public async Task<Result<long>> Handle(CreateSecKillPayLogCommand request, CancellationToken cancellationToken)
    {
        var payLog = await dbContext.PayLogs.FirstOrDefaultAsync(x => x.OrderId == request.OrderId,
            cancellationToken: cancellationToken);
      
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
        
        var response = await seckillService.ServiceApi.GetOrderAsync(user.Id);
        
        if (!response.IsSuccessStatusCode) return Result.Failure("订单不存在");
        
        var order = response.Content!;
        
        payLog = new PayLog(request.OrderId, Convert.ToInt64(order.ActualPay), Convert.ToInt64(user.Id));
        
        dbContext.PayLogs.Add(payLog);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success(payLog.Id);
    }
}