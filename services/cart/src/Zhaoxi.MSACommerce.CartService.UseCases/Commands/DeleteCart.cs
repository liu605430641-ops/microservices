using Zhaoxi.MSACommerce.CartService.Core.Data;
using Zhaoxi.MSACommerce.CartService.Core.Entities;

namespace Zhaoxi.MSACommerce.CartService.UseCases.Commands;

public record DeleteCartCommand(long UserId) : ICommand<Result>;

public class DeleteCartCommandHandler(
    ICartRepository cartRepository,
    IMapper mapper) : ICommandHandler<DeleteCartCommand, Result>
{
    public async Task<Result> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        await cartRepository.ClearCartAsync(request.UserId);
        return Result.Success();
    }
}