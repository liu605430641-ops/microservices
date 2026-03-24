using Zhaoxi.MSACommerce.CartService.Core.Data;
using Zhaoxi.MSACommerce.CartService.Core.Entities;

namespace Zhaoxi.MSACommerce.CartService.UseCases.Queries;

public record GetCartQuery(long UserId) : IQuery<Result<CartDto>>;

public class GetCartQueryHandler(
    ICartRepository cartRepository,
    IMapper mapper) : IQueryHandler<GetCartQuery, Result<CartDto>>
{
    public async Task<Result<CartDto>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetCartAsync(request.UserId);
        if (cart is null) return Result.NotFound();
        
        var result = mapper.Map<CartDto>(cart);
        return Result.Success(result);
    }
}