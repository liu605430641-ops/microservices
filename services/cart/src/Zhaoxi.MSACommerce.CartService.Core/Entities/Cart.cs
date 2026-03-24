namespace Zhaoxi.MSACommerce.CartService.Core.Entities;

public class Cart(long userId)
{
    public long UserId { get; private set; } = userId;
    private readonly Dictionary<long, CartItem> _items = new();

    public IReadOnlyCollection<CartItem> Items => _items.Values;

    public void AddOrUpdateItem(CartItem item)
    {
        if (!_items.TryAdd(item.SkuId, item))
        {
            _items[item.SkuId].UpdateQuantity(_items[item.SkuId].Quantity + item.Quantity);
        }
    }

    public void RemoveItem(int productId)
    {
        _items.Remove(productId);
    }

    public void ClearCart()
    {
        _items.Clear();
    }
}
