using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.CategoryService.Core.Entities;

public class Category : BaseAuditEntity, IAggregateRoot
{
    public string Name { get; set; } = null!;
    public long ParentId { get; set; }
    public bool IsParent { get; set; }
    public int Sort { get; set; }
    public ICollection<CategoryBrands> Brands { get; set; }
}