using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.OrderService.Core.Entities;

namespace Zhaoxi.MSACommerce.OrderService.Infrastructure.Data.Configuration;

public class OrderInfoConfiguration : IEntityTypeConfiguration<OrderInfo>
{
    public void Configure(EntityTypeBuilder<OrderInfo> builder)
    {
        builder.ToTable("tb_order_info");

        builder.HasIndex(e => e.Status);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)")
            .HasComment("订单id");

        builder.Property(e => e.CloseTime)
            .HasColumnName("close_time")
            .HasColumnType("datetime")
            .HasComment("交易关闭时间");
        

        builder.Property(e => e.ConsignTime)
            .HasColumnName("consign_time")
            .HasColumnType("datetime")
            .HasComment("发货时间");

        builder.Property(e => e.CreateTime)
            .HasColumnName("create_time")
            .HasColumnType("datetime")
            .HasComment("订单创建时间");

        builder.Property(e => e.EndTime)
            .HasColumnName("end_time")
            .HasColumnType("datetime")
            .HasComment("交易完成时间");

        builder.Property(e => e.PaymentTime)
            .HasColumnName("payment_time")
            .HasColumnType("datetime")
            .HasComment("付款时间");

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasColumnType("int(1)")
            .HasComment("状态：1、未付款 2、已付款,未发货 3、已发货,未确认 4、交易成功 5、交易关闭");
    }
}