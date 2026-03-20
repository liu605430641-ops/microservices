using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure.Data.Configuration;

public class CategoryBrandConfiguration : IEntityTypeConfiguration<CategoryBrand>
{
    public void Configure(EntityTypeBuilder<CategoryBrand> builder)
    {
        builder.ToTable("tb_category_brand");

        builder.Property(e => e.Id)
               .HasColumnName("id")
               .HasColumnType("bigint(20)");
        
        builder.Property(e => e.CategoryId)
               .HasColumnName("category_id")
               .HasColumnType("bigint(20)")
               .HasComment("品类id");

        builder.Property(e => e.BrandId)
               .HasColumnName("brand_id")
               .HasColumnType("bigint(20)")
               .HasComment("品牌id");
        
        // 联合唯一约束
        builder.HasIndex(e => new { e.CategoryId, e.BrandId })
               .IsUnique();
    }
}