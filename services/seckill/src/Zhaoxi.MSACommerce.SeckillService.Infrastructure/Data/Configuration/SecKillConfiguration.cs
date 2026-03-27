using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhaoxi.MSACommerce.SeckillService.Core.Entities;

namespace Zhaoxi.MSACommerce.SeckillService.Infrastructure.Data.Configuration;

public class SecKillConfiguration : IEntityTypeConfiguration<SecKillProduct>
{
    public void Configure(EntityTypeBuilder<SecKillProduct> builder)
    {
        builder.ToTable("tb_seckill_product");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint(20)")
            .ValueGeneratedNever();
        
        builder.Property(e => e.SkuId)
            .HasColumnName("sku_id")
            .HasColumnType("bigint(20)");
        
        builder.Property(e => e.SpuId)
            .HasColumnName("spu_id")
            .HasColumnType("bigint(20)");
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasColumnType("varchar(128)")
            .HasComment("商品名称");
        
        builder.Property(e => e.Introduction)
            .HasMaxLength(2000)
            .HasColumnName("introduction")
            .HasComment("描述");

        builder.Property(e => e.SmallPic)
            .HasColumnName("small_pic")
            .HasColumnType("varchar(1024)")
            .HasComment("商品缩略图");
        
        builder.Property(e => e.Price)
            .HasColumnName("price")
            .HasColumnType("bigint(15)")
            .HasComment("原价格");
        
        builder.Property(e => e.CostPrice)
            .HasColumnType("bigint(15)")
            .HasColumnName("cost_price")
            .HasComment("秒杀价格");
        
        builder.Property(e => e.Num)
            .HasColumnName("num")
            .HasComment("秒杀商品数");
        
        builder.Property(e => e.StockCount)
            .HasColumnName("stock_count")
            .HasComment("剩余库存数");
        
        builder.Property(e => e.StartTime)
            .HasColumnType("datetime")
            .HasColumnName("start_time")
            .HasComment("开始时间");
        
        builder.Property(e => e.EndTime)
            .HasColumnType("datetime")
            .HasColumnName("end_time")
            .HasComment("结束时间");
    }
}