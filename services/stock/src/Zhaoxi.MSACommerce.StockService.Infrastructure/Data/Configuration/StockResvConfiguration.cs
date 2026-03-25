using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Zhaoxi.MSACommerce.StockService.Core.Entities;

namespace Zhaoxi.MSACommerce.StockService.Infrastructure.Data.Configuration;

public class StockResvConfiguration : IEntityTypeConfiguration<StockResv>
{
    public void Configure(EntityTypeBuilder<StockResv> builder)
    {
        builder.ToTable("tb_stock_resv");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)")
            .HasComment("预留记录id");
        
        builder.Property(e => e.OrderId)
            .HasColumnName("order_id")
            .HasColumnType("bigint(20)")
            .HasComment("关联订单ID");
        
        builder.Property(e => e.ResvQty)
            .HasColumnName("resv_qty")
            .HasColumnType("bigint(20)")
            .HasComment("预留数量");
        
        builder.Property(e => e.ExprTime)
            .HasColumnName("expr_time")
            .HasColumnType("datetime(6)")
            .HasComment("预留过期时间");
    }
}