using Microsoft.EntityFrameworkCore;

namespace Zhaoxi.MSACommerce.OrderService.UseCases.Queries;

public record GetOrderQuery(long OrderId) : IQuery<Result<OrderDto>>;

public class GetOrderQueryHandler(OrderDbContext dbContext, IMapper mapper) : IQueryHandler<GetOrderQuery, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId,
            cancellationToken: cancellationToken);

        if (order == null) return Result.NotFound();
        
        var orderDto = mapper.Map<OrderDto>(order);
        
        return Result.Success(orderDto);
    }
}