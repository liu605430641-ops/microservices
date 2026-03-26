using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.UserService.Core.Entities;

public class TbUser : BaseAuditEntity, IAggregateRoot
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Phone { get; set; }
    public string Salt { get; set; } = null!;
}
