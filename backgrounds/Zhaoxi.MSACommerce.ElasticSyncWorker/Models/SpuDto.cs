namespace Zhaoxi.MSACommerce.ElasticSyncWorker.Models;

public record SpuDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public long CategoryId { get; set; }
    public long BrandId { get; set; }
    public int Status { get; set; }
    public SpuDetailDto Detail { get; set; }
    public IEnumerable<SkuDto> Skus { get; set; }
}