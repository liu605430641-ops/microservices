namespace Zhaoxi.MSACommerce.SharedKernel.Domain;

public abstract class BaseAuditEntity : BaseEntity<long>
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
}
