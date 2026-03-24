using Zhaoxi.MSACommerce.CartService.Core.Entities;

namespace Zhaoxi.MSACommerce.CartService.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Cart, CartDto>();

        CreateMap<CartItem, CartItemDto>();
        
        CreateMap<CartItemDto, CartItem>();
    }
}
