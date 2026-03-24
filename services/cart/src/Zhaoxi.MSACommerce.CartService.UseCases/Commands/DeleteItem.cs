using Zhaoxi.MSACommerce.CartService.Core.Data;
using Zhaoxi.MSACommerce.CartService.Core.Entities;

namespace Zhaoxi.MSACommerce.CartService.UseCases.Commands;

public record DeleteItemCommand(long UserId, long SkuId) : ICommand<Result>;

public class DeleteItemCommandHandler(
    ICartRepository cartRepository,
    IMapper mapper) : ICommandHandler<DeleteItemCommand, Result>
{
    public async Task<Result> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        await cartRepository.RemoveItemAsync(request.UserId, request.SkuId);
        return Result.Success();
    }
}