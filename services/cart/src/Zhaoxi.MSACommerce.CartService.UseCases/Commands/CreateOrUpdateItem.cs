using Zhaoxi.MSACommerce.CartService.Core.Data;
using Zhaoxi.MSACommerce.CartService.Core.Entities;

namespace Zhaoxi.MSACommerce.CartService.UseCases.Commands;

public record CreateOrUpdateItemCommand(long UserId, CartItemDto Item) : ICommand<Result>;

public class CreateOrUpdateItemCommandHandler(
    ICartRepository cartRepository,
    IMapper mapper) : ICommandHandler<CreateOrUpdateItemCommand, Result>
{
    public async Task<Result> Handle(CreateOrUpdateItemCommand request, CancellationToken cancellationToken)
    {
        var cartItem = mapper.Map<CartItem>(request.Item);
        await cartRepository.AddOrUpdateItemAsync(request.UserId, cartItem);

        return Result.Success();
    }
}