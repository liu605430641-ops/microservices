using Zhaoxi.MSACommerce.CategoryService.Core.Enums;
using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.CategoryService.Core.Entities;

public class Sku : BaseAuditEntity
{
    public long SpuId { get; set; }
    
    public string Name { get; set; } = null!;

    public string? Images { get; set; }
    
    public long Price { get; set; }

    public string Indexes { get; set; } = null!;
    
    public string Spec { get; set; } = null!;
    
    public Status Status { get; set; }
}