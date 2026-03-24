namespace Zhaoxi.MSACommerce.SharedKernel.Domain;

public abstract class AuditWithUserEntity : BaseAuditEntity
{
    public long? CreatedBy { get; set; }
    public long? LastModifiedBy { get; set; }
}
