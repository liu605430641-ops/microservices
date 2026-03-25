using Zhaoxi.MSACommerce.CategoryService.Core.Enmus;
using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.CategoryService.Core.Entities;

public class PayLog : BaseAuditEntity
{
    protected PayLog()
    {
    }

    public PayLog(long orderId,long totalFee,long userId)
    {
        OrderId  = orderId;
        TotalFee = totalFee;
        UserId   = userId;
        Status   = PayStatus.UnPay;
    }

    public long      OrderId  { get; set; }
    public long      TotalFee { get; set; }
    public long      UserId   { get; set; }
    public PayStatus Status   { get; set; }
    public DateTime? PayTime  { get; set; }
}