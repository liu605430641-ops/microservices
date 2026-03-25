using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.OrderService.Core.Entities;

namespace Zhaoxi.MSACommerce.OrderService.Infrastructure.Data.Configuration;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("tb_order_detail");

        builder.HasIndex(e => e.OrderId);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)")
            .HasComment("订单明细项id");

        builder.Property(e => e.Image)
            .HasColumnName("image")
            .HasColumnType("varchar(128)")
            .HasDefaultValueSql("''")
            .HasComment("商品图片");

        builder.Property(e => e.Quantity)
            .HasColumnName("quantity")
            .HasColumnType("int(11)")
            .HasComment("购买数量");

        builder.Property(e => e.OrderId)
            .HasColumnName("order_id")
            .HasColumnType("bigint(20)")
            .HasComment("订单id");

        builder.Property(e => e.Spec)
            .HasColumnName("own_spec")
            .HasColumnType("varchar(1024)")
            .HasDefaultValueSql("''")
            .HasComment("商品动态属性键值集");

        builder.Property(e => e.Price)
            .HasColumnName("price")
            .HasColumnType("bigint(20)")
            .HasComment("价格,单位：分");

        builder.Property(e => e.SkuId)
            .HasColumnName("sku_id")
            .HasColumnType("bigint(20)")
            .HasComment("sku商品id");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName("title")
            .HasColumnType("varchar(256)")
            .HasComment("商品标题");
    }
}