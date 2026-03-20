using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.ProductService.Infrastructure.Data;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) :  DbContext(options)
{
    public DbSet<Spu> Spus => Set<Spu>();
    public DbSet<SpuDetail> SpuDetails => Set<SpuDetail>();
    public DbSet<Sku> Skus => Set<Sku>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
