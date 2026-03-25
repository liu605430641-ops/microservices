using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Enmus;
using Zhaoxi.MSACommerce.PaymentService.Infrastructure.Data;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.Queries;

public record PayStatusDto(PayStatus Status);

public record GetPayStatusQuery(long OrderId) : IQuery<Result>;

public class GetPayStatusQueryHandler(PaymentDbContext dbContext)
    : IQueryHandler<GetPayStatusQuery,Result>
{
    public async Task<Result> Handle(GetPayStatusQuery request,
                                     CancellationToken cancellationToken)
    {
        var payStatus = await dbContext.PayLogs
                                       .Where(p => p.OrderId == request.OrderId)
                                       .Select(p => new PayStatusDto(p.Status))
                                       .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return payStatus is null ? Result.NotFound() : Result.Success();
    }
}