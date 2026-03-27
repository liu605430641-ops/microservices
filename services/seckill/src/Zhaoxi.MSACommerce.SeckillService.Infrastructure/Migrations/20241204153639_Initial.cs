using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhaoxi.MSACommerce.SeckillService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_seckill_product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false),
                    spu_id = table.Column<long>(type: "bigint(20)", nullable: false),
                    sku_id = table.Column<long>(type: "bigint(20)", nullable: false),
                    name = table.Column<string>(type: "varchar(128)", nullable: false, comment: "商品名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    small_pic = table.Column<string>(type: "varchar(1024)", nullable: false, comment: "商品缩略图")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<long>(type: "bigint(15)", nullable: false, comment: "原价格"),
                    cost_price = table.Column<long>(type: "bigint(15)", nullable: false, comment: "秒杀价格"),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "开始时间"),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "结束时间"),
                    num = table.Column<int>(type: "int", nullable: false, comment: "秒杀商品数"),
                    stock_count = table.Column<int>(type: "int", nullable: false, comment: "剩余库存数"),
                    introduction = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true, comment: "描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_seckill_product", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_seckill_product");
        }
    }
}
