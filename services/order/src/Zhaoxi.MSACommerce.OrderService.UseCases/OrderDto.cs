using Zhaoxi.MSACommerce.OrderService.Core.Enums;

namespace Zhaoxi.MSACommerce.OrderService.UseCases;

public record OrderDto
{
    public long Id { get; set; }
    
    // 总金额
    public long TotalPay { get; set; }
    
    // 实付金额
    public long ActualPay { get; set; }
    
    // 付款方式
    public PaymentType PaymentType { get; set; }
    
    // 用户ID
    public long UserId { get; set; }
    
    // 收货地址
    public string ReceiverAddress { get; set; } = null!;
    
    // 收货人
    public string Receiver { get; set; } = null!;
}