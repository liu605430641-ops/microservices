using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.OrderService.Core.Entities;

namespace Zhaoxi.MSACommerce.OrderService.Infrastructure.Data;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) :  DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderInfo> OrderInfos => Set<OrderInfo>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}