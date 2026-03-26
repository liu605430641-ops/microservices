using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Enmus;
using Zhaoxi.MSACommerce.PaymentService.Infrastructure.Data;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.Queries;

public record PayStatusDto(PayStatus Status);

public record GetPayStatusQuery(long OrderId) : IQuery<Result<PayStatusDto>>;

public class GetPayStatusQueryHandler(PaymentDbContext dbContext)
    : IQueryHandler<GetPayStatusQuery, Result<PayStatusDto>>
{
    public async Task<Result<PayStatusDto>> Handle(GetPayStatusQuery request,
        CancellationToken cancellationToken)
    {
        var payStatus = await dbContext.PayLogs
            .Where(p => p.OrderId == request.OrderId)
            .Select(p => new PayStatusDto(p.Status))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return payStatus is null ? Result.NotFound() : Result.Success(payStatus);
    }
}