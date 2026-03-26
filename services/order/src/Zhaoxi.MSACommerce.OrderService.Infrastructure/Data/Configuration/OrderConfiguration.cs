using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.OrderService.Core.Entities;

namespace Zhaoxi.MSACommerce.OrderService.Infrastructure.Data.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("tb_order");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)")
            .HasComment("订单id")
            .ValueGeneratedNever();

        builder.Property(e => e.ActualPay)
            .HasColumnName("actual_pay")
            .HasColumnType("bigint(20)")
            .HasComment("实付金额。单位:分。如:20007，表示:200元7分");


        builder.Property(e => e.Receiver)
            .HasColumnName("receiver")
            .HasColumnType("varchar(32)")
            .HasComment("收货人");

        builder.Property(e => e.ReceiverAddress)
            .HasColumnName("receiver_address")
            .HasColumnType("varchar(256)")
            .HasDefaultValueSql("''")
            .HasComment("收获地址（街道、住址等详细地址）")
            .HasCharSet("utf8")
            .UseCollation("utf8_bin");

        builder.Property(e => e.TotalPay)
            .HasColumnName("total_pay")
            .HasColumnType("bigint(20)")
            .HasComment("总金额，单位为分");
        
        builder.Property(e => e.PaymentType)
            .HasColumnName("payment_type")
            .HasColumnType("int(1)")
            .HasComment("1、支付宝；2、微信支付");
        
        builder.Property(e => e.UserId)
            .IsRequired()
            .HasColumnName("user_id")
            .HasColumnType("varchar(32)")
            .HasComment("用户id");

        builder.HasMany(e => e.OrderDetails)
            .WithOne(e => e.Order)
            .HasForeignKey(e => e.OrderId);

        builder.HasOne(e => e.OrderInfo)
            .WithOne(e => e.Order)
            .HasForeignKey<OrderInfo>(e => e.Id);
    }
}