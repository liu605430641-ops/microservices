using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhaoxi.MSACommerce.OrderService.Infrastructure.Migrations
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
                name: "tb_order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "订单id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    total_pay = table.Column<long>(type: "bigint(20)", nullable: false, comment: "总金额，单位为分"),
                    actual_pay = table.Column<long>(type: "bigint(20)", nullable: false, comment: "实付金额。单位:分。如:20007，表示:200元7分"),
                    payment_type = table.Column<int>(type: "int(1)", nullable: false, comment: "1、支付宝；2、微信支付"),
                    user_id = table.Column<string>(type: "varchar(32)", nullable: false, comment: "用户id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    receiver_address = table.Column<string>(type: "varchar(256)", nullable: false, defaultValueSql: "''", comment: "收获地址（街道、住址等详细地址）", collation: "utf8_bin")
                        .Annotation("MySql:CharSet", "utf8"),
                    receiver = table.Column<string>(type: "varchar(32)", nullable: false, comment: "收货人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_order", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_order_detail",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "订单明细项id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    order_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "订单id"),
                    sku_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "sku商品id"),
                    quantity = table.Column<int>(type: "int(11)", nullable: false, comment: "购买数量"),
                    title = table.Column<string>(type: "varchar(256)", nullable: false, comment: "商品标题")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    own_spec = table.Column<string>(type: "varchar(1024)", nullable: false, defaultValueSql: "''", comment: "商品动态属性键值集")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<long>(type: "bigint(20)", nullable: false, comment: "价格,单位：分"),
                    image = table.Column<string>(type: "varchar(128)", nullable: true, defaultValueSql: "''", comment: "商品图片")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_order_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_order_detail_tb_order_order_id",
                        column: x => x.order_id,
                        principalTable: "tb_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_order_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "订单id"),
                    status = table.Column<int>(type: "int(1)", nullable: false, comment: "状态：1、未付款 2、已付款,未发货 3、已发货,未确认 4、交易成功 5、交易关闭"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "订单创建时间"),
                    payment_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "付款时间"),
                    consign_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "发货时间"),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "交易完成时间"),
                    close_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "交易关闭时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_order_info", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_order_info_tb_order_id",
                        column: x => x.id,
                        principalTable: "tb_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_order_detail_order_id",
                table: "tb_order_detail",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_order_info_status",
                table: "tb_order_info",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_order_detail");

            migrationBuilder.DropTable(
                name: "tb_order_info");

            migrationBuilder.DropTable(
                name: "tb_order");
        }
    }
}
