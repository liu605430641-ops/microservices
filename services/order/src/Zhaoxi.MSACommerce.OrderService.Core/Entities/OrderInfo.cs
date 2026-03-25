using Zhaoxi.MSACommerce.OrderService.Core.Enums;
using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.OrderService.Core.Entities;

public class OrderInfo : BaseEntity<long>
{
    // 订单状态 1、未付款 2、已付款未发货 3、已发货未确认 4、交易成功 5、交易关闭
    public OrderStatus Status { get; set; }
        
    // 订单创建时间
    public DateTime? CreateTime { get; set; }
    // 支付时间
    public DateTime? PaymentTime { get; set; }
    // 发货时间
    public DateTime? ConsignTime { get; set; }
    // 交易完成时间
    public DateTime? EndTime { get; set; }
    // 交易关闭时间
    public DateTime? CloseTime { get; set; }

    public Order Order { get; set; } = new();
}