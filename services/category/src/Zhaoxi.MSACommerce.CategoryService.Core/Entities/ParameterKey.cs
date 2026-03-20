using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.CategoryService.Core.Entities;

public class ParameterKey : BaseAuditEntity
{
    public string Name { get; set; } = null!;
    
    public long ParameterGroupId { get; set; }
    
    public ParameterGroup ParameterGroup { get; set; }
    
    public long CategoryId { get; set; }
}