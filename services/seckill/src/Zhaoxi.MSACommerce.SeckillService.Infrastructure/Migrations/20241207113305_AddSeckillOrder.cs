using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhaoxi.MSACommerce.SeckillService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeckillOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "stock_count",
                table: "tb_seckill_product",
                type: "bigint",
                nullable: false,
                comment: "剩余库存数",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "剩余库存数");

            migrationBuilder.CreateTable(
                name: "tb_seckill_order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false),
                    seckill_id = table.Column<long>(type: "bigint", nullable: false, comment: "秒杀商品ID"),
                    money = table.Column<long>(type: "bigint", precision: 10, scale: 2, nullable: false, comment: "支付金额"),
                    user_id = table.Column<long>(type: "bigint", maxLength: 50, nullable: false, comment: "用户"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    pay_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "支付时间"),
                    status = table.Column<int>(type: "int", fixedLength: true, maxLength: 1, nullable: false, comment: "状态，0未支付，1已支付"),
                    receiver_address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "收货人地址")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    receiver_mobile = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "收货人电话")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    receiver = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "收货人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    transaction_id = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, comment: "交易流水")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_seckill_order", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_seckill_order");

            migrationBuilder.AlterColumn<int>(
                name: "stock_count",
                table: "tb_seckill_product",
                type: "int",
                nullable: false,
                comment: "剩余库存数",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "剩余库存数");
        }
    }
}
