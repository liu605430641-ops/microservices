using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.StockService.Core.Entities;

namespace Zhaoxi.MSACommerce.StockService.Infrastructure.Data;

public class StockDbContext(DbContextOptions<StockDbContext> options) :  DbContext(options)
{
    public DbSet<SkuStock> SkuStocks => Set<SkuStock>();
    public DbSet<StockResv> StockResvs => Set<StockResv>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}