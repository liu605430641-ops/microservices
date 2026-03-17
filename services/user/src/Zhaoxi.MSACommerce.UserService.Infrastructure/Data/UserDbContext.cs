using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.UserService.Core.Entites;

namespace Zhaoxi.MSACommerce.UserService.Infrastructure.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) :  DbContext(options)
{
    public DbSet<TbUser> TbUsers => Set<TbUser>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
