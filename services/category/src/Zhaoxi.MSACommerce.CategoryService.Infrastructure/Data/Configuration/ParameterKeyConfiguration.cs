using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure.Data.Configuration;

public class ParameterKeyConfiguration : IEntityTypeConfiguration<ParameterKey>
{
    public void Configure(EntityTypeBuilder<ParameterKey> builder)
    {
        builder.ToTable("tb_param_key");
        
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)")
            .HasComment("参数Id");

        builder.Property(s => s.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasColumnType("varchar(256)")
            .HasComment("参数名称");
        
        builder.Property(s => s.ParameterGroupId)
            .HasColumnName("param_group_id")
            .HasColumnType("bigint(20)")
            .HasComment("所属分组");
        
        builder.Property(s => s.CategoryId)
            .HasColumnName("category_Id")
            .HasColumnType("bigint(20)")
            .HasComment("所属分类");

        builder.HasOne(s => s.ParameterGroup)
            .WithMany(s => s.ParameterKeys);

        builder.HasIndex(s => s.CategoryId);
    }
}