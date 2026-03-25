using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhaoxi.MSACommerce.StockService.Infrastructure.Migrations
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
                name: "tb_sku_stock",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "商品SKU Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    total_qty = table.Column<long>(type: "bigint(20)", nullable: false, comment: "库存总数量"),
                    avail_qty = table.Column<long>(type: "bigint(20)", nullable: false, comment: "可用数量"),
                    resv_qty = table.Column<long>(type: "bigint(20)", nullable: false, comment: "预留数量")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sku_stock", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_stock_resv",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "预留记录id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    order_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "关联订单ID"),
                    resv_qty = table.Column<long>(type: "bigint(20)", nullable: false, comment: "预留数量"),
                    expr_time = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "预留过期时间"),
                    SkuId = table.Column<long>(type: "bigint(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_stock_resv", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_stock_resv_tb_sku_stock_SkuId",
                        column: x => x.SkuId,
                        principalTable: "tb_sku_stock",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_stock_resv_SkuId",
                table: "tb_stock_resv",
                column: "SkuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_stock_resv");

            migrationBuilder.DropTable(
                name: "tb_sku_stock");
        }
    }
}
