using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure.Data.Configuration;

public class ParameterGroupConfiguration : IEntityTypeConfiguration<ParameterGroup>
{
    public void Configure(EntityTypeBuilder<ParameterGroup> builder)
    {
        builder.ToTable("tb_param_group");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)");
        
        builder.Property(e => e.CategoryId)
            .HasColumnName("category_id")
            .HasColumnType("bigint(20)")
            .HasComment("所属品类");

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(32)")
            .HasComment("参数组名");
        
        builder.HasIndex(s => s.CategoryId);
    }
}