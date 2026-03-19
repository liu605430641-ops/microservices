using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.CategoryService.Core.Entities;

public class CategoryBrands : BaseAuditEntity
{
    public long CategoryId { get; set; }
    public long BrandId { get; set; }
    
    public List<Category> Categories { get; set; }
}