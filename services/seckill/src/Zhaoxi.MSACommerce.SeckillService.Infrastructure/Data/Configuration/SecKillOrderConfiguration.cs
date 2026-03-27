using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.SeckillService.Core.Entities;

namespace Zhaoxi.MSACommerce.SeckillService.Infrastructure.Data.Configuration;

public class SecKillOrderConfiguration : IEntityTypeConfiguration<SeckillOrder>
{
    public void Configure(EntityTypeBuilder<SeckillOrder> builder)
    {
        builder.ToTable("tb_seckill_order");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)")
            .ValueGeneratedNever();
        
        builder.Property(e => e.CreateTime)
            .HasColumnType("datetime")
            .HasColumnName("create_time")
            .HasComment("创建时间");

        builder.Property(e => e.ActualPay)
            .HasPrecision(10, 2)
            .HasColumnName("money")
            .HasComment("支付金额");

        builder.Property(e => e.PayTime)
            .HasColumnType("datetime")
            .HasColumnName("pay_time")
            .HasComment("支付时间");

        builder.Property(e => e.Receiver)
            .HasMaxLength(20)
            .HasColumnName("receiver")
            .HasComment("收货人");

        builder.Property(e => e.ReceiverAddress)
            .HasMaxLength(200)
            .HasColumnName("receiver_address")
            .HasComment("收货人地址");

        builder.Property(e => e.ReceiverMobile)
            .HasMaxLength(20)
            .HasColumnName("receiver_mobile")
            .HasComment("收货人电话");

        builder.Property(e => e.SeckillId)
            .HasColumnName("seckill_id")
            .HasComment("秒杀商品ID");

        builder.Property(e => e.Status)
            .HasMaxLength(1)
            .HasColumnName("status")
            .IsFixedLength(true)
            .HasComment("状态，0未支付，1已支付");

        builder.Property(e => e.TransactionId)
            .HasMaxLength(30)
            .HasColumnName("transaction_id")
            .HasComment("交易流水");

        builder.Property(e => e.UserId)
            .HasMaxLength(50)
            .HasColumnName("user_id")
            .HasComment("用户");
    }
}