using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.OrderService.Core.Entities;

public class Order : BaseAuditEntity
{
    // 总金额
    public long TotalPay { get; set; }
    
    // 实付金额
    public long ActualPay { get; set; }
    
    // 用户ID
    public long UserId { get; set; }
    
    // 收货地址
    public string ReceiverAddress { get; set; } = null!;
    
    // 收货人
    public string Receiver { get; set; } = null!;

    public OrderInfo OrderInfo { get; set; } = new();

    public ICollection<OrderDetail> OrderDetails = new List<OrderDetail>();
    
}