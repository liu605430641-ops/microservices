using System.Text.Json.Serialization;

namespace Zhaoxi.MSACommerce.OrderService.UseCases;

public record SkuDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Images { get; set; }
    public long Price { get; set; }
    // [JsonConverter(typeof(NonEscapedStringConverter))]
    public dynamic Spec { get; set; } = null!;
}