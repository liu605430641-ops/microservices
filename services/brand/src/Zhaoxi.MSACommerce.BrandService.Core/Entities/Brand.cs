using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.CategoryService.Core.Entities;

public class Brand : BaseAuditEntity, IAggregateRoot
{
    public string Name { get; set; } = null!;

    public string? Image { get; set; }
    
    public string Letter { get; set; } = null!;
}