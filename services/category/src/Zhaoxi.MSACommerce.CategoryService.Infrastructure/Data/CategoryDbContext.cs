using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure.Data;

public class CategoryDbContext(DbContextOptions<CategoryDbContext> options) :  DbContext(options)
{
    public DbSet<Category> Category => Set<Category>();
    public DbSet<CategoryBrands> CategoryBrands => Set<CategoryBrands>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
