using Consul;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.ProductService.Infrastructure.Data.Configuration;

public class SpuConfiguration : IEntityTypeConfiguration<Spu>
{
    public void Configure(EntityTypeBuilder<Spu> builder)
    {
        builder.ToTable("tb_spu");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)");
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasColumnType("varchar(128)")
            .HasComment("产品名称");

        builder.Property(e => e.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasColumnType("varchar(256)")
            .HasComment("产品描述");
        
        builder.Property(e => e.CategoryId)
            .HasColumnName("category_id")
            .HasColumnType("bigint(20)")
            .HasComment("所属品类id");

        builder.Property(e => e.BrandId)
            .HasColumnName("brand_id")
            .HasColumnType("bigint(20)")
            .HasComment("商品所属品牌id");
        
        builder.Property(e => e.Status)
            .IsRequired()
            .HasColumnName("status")
            .HasComment("是否在售，0未售，1在售");

        builder.HasMany(e => e.Skus)
            .WithOne()
            .HasForeignKey(e => e.SpuId);

        builder.HasOne(e => e.Detail)
            .WithOne()
            .HasForeignKey<SpuDetail>();
    }
}