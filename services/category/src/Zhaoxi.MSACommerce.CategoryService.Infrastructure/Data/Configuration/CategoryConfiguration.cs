using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure.Data.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("tb_category");
        
        builder.Property(e => e.Id)
               .HasColumnName("id")
               .HasColumnType("bigint(20)")
               .HasComment("品类id");

        builder.Property(e => e.Name)
               .IsRequired()
               .HasColumnName("name")
               .HasColumnType("varchar(32)")
               .HasComment("品类名称");
        
        builder.Property(e => e.IsParent)
               .HasColumnName("is_parent")
               .HasComment("是否为父节点，0为否，1为是");

        builder.Property(e => e.ParentId)
               .HasColumnName("parent_id")
               .HasColumnType("bigint(20)")
               .HasComment("父类id,顶级类填0");

        builder.Property(e => e.Sort)
               .HasColumnName("sort")
               .HasColumnType("int(4)")
               .HasComment("排序指数，越小越靠前");
    }
}