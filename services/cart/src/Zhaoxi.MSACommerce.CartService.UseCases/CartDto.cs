using Zhaoxi.MSACommerce.CartService.Core.Entities;

namespace Zhaoxi.MSACommerce.CartService.UseCases;

public record CartDto
{
    public long UserId { get; set; }
    public IReadOnlyCollection<CartItem> Items { get; set; }
}

public record CartItemDto(long SkuId, string Name, int Quantity, string? Image, long Price, string? Spec);


