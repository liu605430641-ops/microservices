using Zhaoxi.MSACommerce.CategoryService.Core.Enums;
using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.CategoryService.Core.Entities;

public class Spu : BaseAuditEntity, IAggregateRoot
{
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public long CategoryId { get; set; }
    
    public long BrandId { get; set; }

    public Status Status { get; set; }
    
    public SpuDetail Detail { get; set; }
    
    public ICollection<Sku> Skus { get; set; }
}

