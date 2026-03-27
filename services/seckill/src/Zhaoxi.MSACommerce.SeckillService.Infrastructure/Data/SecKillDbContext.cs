using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.SeckillService.Core.Entities;

namespace Zhaoxi.MSACommerce.SeckillService.Infrastructure.Data;

public class SecKillDbContext(DbContextOptions<SecKillDbContext> options) :  DbContext(options)
{
    public DbSet<SecKillProduct> SecKillProducts => Set<SecKillProduct>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
