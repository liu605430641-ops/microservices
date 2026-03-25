using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.OrderService.Core.Entities;
using Zhaoxi.MSACommerce.OrderService.UseCases.Apis;

namespace Zhaoxi.MSACommerce.OrderService.UseCases.Commands;

public record CreateOrderCommand(IList<SkuItemDto> Items) : ICommand<Result>;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(query => query.Items)
            .Must(x => x.Count > 0);
    }
}

public class CreateOrderCommandHandler(OrderDbContext dbContext, IServiceClient<IStockServiceApi> stockClient) : ICommandHandler<CreateOrderCommand, Result>
{
    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order();
        foreach (var itemDto in request.Items)
        {
            await stockClient.ServiceApi.CreateStockResvAsync(itemDto.SkuId, order.Id, itemDto.Quantity);
            
            order.OrderDetails.Add(new OrderDetail
            {
                SkuId = itemDto.SkuId,
                Quantity = itemDto.Quantity
            });
            
        }

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}