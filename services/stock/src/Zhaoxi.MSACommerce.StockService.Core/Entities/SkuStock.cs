using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.StockService.Core.Entities;

public class SkuStock : BaseEntity<long>
{
    // 总库存数量
    public long TotalQty { get; set; }
    
    // 可用库存数量
    public long AvailQty { get; set; }
    
    // 预留库存数量
    public long ResvQty  { get; set; }
    
    // 预留库存记录
    public ICollection<StockResv> StockResve { get; set; } = new List<StockResv>();
    
    public void AddResvQty(long orderId, long qty, int exprMinutes)
    {
        AvailQty -= qty;
        ResvQty += qty;
        StockResve.Add(new StockResv(orderId, qty, DateTime.Now.AddMinutes(exprMinutes)));
    }
    
    public void ApplyResvQty(long qty)
    {
        TotalQty -= qty;
        ResvQty -= qty;
    }
    
    public void ReleseResvQty(long qty)
    {
        AvailQty += qty;
        ResvQty -= qty;
    }
}