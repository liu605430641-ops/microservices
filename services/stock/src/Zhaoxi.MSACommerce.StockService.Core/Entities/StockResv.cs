using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.StockService.Core.Entities;

public class StockResv : BaseEntity<long>
{
    // 订单ID
    public long OrderId { get; set; }
    
    // 预留数量
    public long ResvQty { get; set; }
    
    // 预留时间
    public DateTime ExprTime { get; set; }
    
    // 商品SKU ID
    public long SkuId { get; set; }
    
    public SkuStock SkuStock { get; set; }
    
    public StockResv(long orderId, long resvQty, DateTime exprTime)
    {
        OrderId = orderId;
        ResvQty = resvQty;
        ExprTime = exprTime;
    }
}