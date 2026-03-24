using Zhaoxi.MSACommerce.CartService.Core.Entities;

namespace Zhaoxi.MSACommerce.CartService.Core.Data;

public interface ICartRepository
{
    Task<Cart?> GetCartAsync(long userId);
    Task AddOrUpdateItemAsync(long userId, CartItem item);
    Task RemoveItemAsync(long userId, long skuId);
    Task ClearCartAsync(long userId);
}
