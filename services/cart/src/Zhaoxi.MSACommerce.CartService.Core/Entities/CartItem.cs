namespace Zhaoxi.MSACommerce.CartService.Core.Entities;

public class CartItem(long skuId, string name, int quantity)
{
    public long SkuId { get; private set; } = skuId;
    public string Name { get; private set; } = name;
    public int Quantity { get; private set; } = quantity;
    public string? Image { get; set; }
    public long Price { get; set; }
    public string? Spec { get; set; }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
}
