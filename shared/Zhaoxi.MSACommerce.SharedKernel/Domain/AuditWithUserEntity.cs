namespace Zhaoxi.MSACommerce.SharedKernel.Domain;

public abstract class AuditWithUserEntity : BaseAuditEntity
{
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
}
