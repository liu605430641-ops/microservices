namespace Zhaoxi.MSACommerce.ElasticSyncWorker.Models
{
    public record SkuDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Images { get; set; }
        public long Price { get; set; }
        public string Indexes { get; set; } = null!;
        public dynamic Spec { get; set; } = null!;
        public int Status { get; set; }
    }
}