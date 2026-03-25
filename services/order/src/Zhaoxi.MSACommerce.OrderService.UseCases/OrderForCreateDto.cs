using Zhaoxi.MSACommerce.OrderService.Core.Enums;

namespace Zhaoxi.MSACommerce.OrderService.UseCases;

public record CartDto(long SkuId, int Quantity);

public record OrderForCreateDto
{
    public long AddressId { get; set; }
    
    public List<CartDto> Carts { get; set; }
    
    public PaymentType PaymentType { get; set; }
};

