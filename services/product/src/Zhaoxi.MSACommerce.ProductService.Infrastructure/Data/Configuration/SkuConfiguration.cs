using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.ProductService.Infrastructure.Data.Configuration;

public class SkuConfiguration : IEntityTypeConfiguration<Sku>
{
    public void Configure(EntityTypeBuilder<Sku> builder)
    {
        builder.ToTable("tb_sku");

        builder.Property(e => e.SpuId)
            .HasColumnName("spu_id")
            .HasColumnType("bigint(20)");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)");
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasColumnType("varchar(32)")
            .HasComment("商品名称");

        builder.Property(e => e.Images)
            .HasColumnName("images")
            .HasColumnType("varchar(1024)")
            .HasComment("商品的图片，多个图片以‘,’分割");

        builder.Property(e => e.Indexes)
            .HasColumnName("indexes")
            .HasColumnType("varchar(32)")
            .HasDefaultValueSql("''")
            .HasComment("sku规格在spu规格中的对应下标组合");

        builder.Property(e => e.Spec)
            .HasColumnName("spec")
            .HasColumnType("varchar(1024)")
            .HasDefaultValueSql("''")
            .HasComment("规格参数键值对，json格式");

        builder.Property(e => e.Status)
            .IsRequired()
            .HasColumnName("status");
        
        builder.Property(e => e.Price)
            .HasColumnName("price")
            .HasColumnType("bigint(15)")
            .HasComment("销售价格，单位为分");
    }
}