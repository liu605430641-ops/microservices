using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.CategoryService.Core.Entities;

public class CategoryBrand : BaseAuditEntity
{
    public long CategoryId { get; set; }
    public long BrandId { get; set; }
}