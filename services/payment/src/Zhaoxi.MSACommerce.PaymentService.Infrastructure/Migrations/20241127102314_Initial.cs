using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhaoxi.MSACommerce.PaymentService.Infrastructure.Migrations
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
                name: "tb_pay_log",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    order_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "订单号"),
                    total_fee = table.Column<long>(type: "bigint(20)", nullable: false, comment: "支付金额（分）"),
                    user_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "用户ID"),
                    status = table.Column<int>(type: "int", nullable: false, comment: "交易状态，1 未支付, 2已支付, 3 已退款, 4 支付错误, 5 已关闭"),
                    pay_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "支付时间"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_pay_log", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_pay_log");
        }
    }
}
