using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.CategoryService.Core.Entities;

public class SpecKey : BaseAuditEntity
{
    public string Name { get; set; } = null!;
    
    public long CategoryId { get; set; }
}