using Zhaoxi.MSACommerce.SeckillService.Core.Enums;
using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.SeckillService.Core.Entities;

public class SeckillOrder : BaseAuditEntity
{
    public long SeckillId { get; set; }
    public long ActualPay { get; set; }
    public long UserId { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public DateTime? PayTime { get; set; }
    public OrderStatus Status { get; set; }
    public string? ReceiverAddress { get; set; }
    public string? ReceiverMobile { get; set; }
    public string? Receiver { get; set; }
    public string? TransactionId { get; set; }
}