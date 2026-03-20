using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure.Data.Configuration;

public class SpecKeyConfiguration : IEntityTypeConfiguration<SpecKey>
{
    public void Configure(EntityTypeBuilder<SpecKey> builder)
    {
        builder.ToTable("tb_spec_key");
        
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)")
            .HasComment("规格Id");

        builder.Property(s => s.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasColumnType("varchar(256)")
            .HasComment("规格名称");
        
        
        builder.Property(s => s.CategoryId)
            .HasColumnName("category_Id")
            .HasColumnType("bigint(20)")
            .HasComment("所属分类");
        

        builder.HasIndex(s => s.CategoryId);
    }
}