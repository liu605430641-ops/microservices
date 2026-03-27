using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.SeckillService.Core.Entities;

public class SecKillProduct : BaseAuditEntity
{
    public long SpuId { get; set; }
    public long SkuId { get; set; }
    public required string Name { get; set; }
    public required string SmallPic { get; set; }
    public long Price { get; set; }
    public long CostPrice { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Num { get; set; }
    public int StockCount { get; set; }
    public string? Introduction { get; set; }
}