using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.CategoryService.Core.Entities;

public class SpuDetail : BaseAuditEntity
{
    public string Introduction { get; set; } = null!;
    public string Spec { get; set; } = null!;
    public string Parameter { get; set; } = null!;
}