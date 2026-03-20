using Newtonsoft.Json;
using Zhaoxi.MSACommerce.CategoryService.Core.Enums;

namespace Zhaoxi.MSACommerce.ProductService.UseCases
{
    public record SpuDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public long CategoryId { get; set; }
        public long BrandId { get; set; }
        public Status Status { get; set; }
        public SpuDetailDto Detail { get; set; }
        public IEnumerable<SkuDto> Skus { get; set; }
    }

    public record SpuDetailDto
    {
        public string Introduction { get; set; } = null!;
        [JsonConverter(typeof(NonEscapedStringConverter))]
        public string Parameter { get; set; } = null!;
        [JsonConverter(typeof(NonEscapedStringConverter))]
        public string Spec { get; set; } = null!;
    }

    public record SkuDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Images { get; set; }
        public long Price { get; set; }
        public string Indexes { get; set; } = null!;
        [JsonConverter(typeof(NonEscapedStringConverter))]
        public string Spec { get; set; } = null!;
        public Status Status { get; set; }
    }
}