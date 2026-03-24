using StackExchange.Redis;
using Zhaoxi.MSACommerce.CartService.Core.Data;
using Zhaoxi.MSACommerce.CartService.Core.Entities;
using System.Text.Json;

namespace Zhaoxi.MSACommerce.CartService.Infrastructure.Data;

public class RedisCartRepository(IDatabase db) : ICartRepository
{
    public async Task<Cart?> GetCartAsync(long userId)
    {
        var cartKey = $"cart:{userId}";
        var entries = await db.HashGetAllAsync(cartKey);
        if (entries.Length == 0) return null;

        var cart = new Cart(userId);
        foreach (var entry in entries)
        {
            var item = JsonSerializer.Deserialize<CartItem>(entry.Value);
            cart.AddOrUpdateItem(item);
        }
        
        return cart;
    }

    public async Task AddOrUpdateItemAsync(long userId, CartItem item)
    {
        var cartKey = $"cart:{userId}";
        var itemJson = await db.HashGetAsync(cartKey, item.SkuId);
        
        if (itemJson.HasValue)
        {
            var itemDb = JsonSerializer.Deserialize<CartItem>(itemJson);
            item.UpdateQuantity(itemDb.Quantity + item.Quantity);
            item.Price = itemDb.Price;
        }
        var itemData = JsonSerializer.Serialize(item);
        await db.HashSetAsync(cartKey, item.SkuId, itemData);
    }

    public async Task RemoveItemAsync(long userId, long skuId)
    {
        var cartKey = $"cart:{userId}";
        await db.HashDeleteAsync(cartKey, skuId);
    }

    public async Task ClearCartAsync(long userId)
    {
        var cartKey = $"cart:{userId}";
        await db.KeyDeleteAsync(cartKey);
    }
}
