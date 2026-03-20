using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhaoxi.MSACommerce.ProductService.Infrastructure.Migrations
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
                name: "tb_spu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(128)", nullable: false, comment: "产品名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(256)", nullable: false, comment: "产品描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    category_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "所属品类id"),
                    brand_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "商品所属品牌id"),
                    status = table.Column<int>(type: "int", nullable: false, comment: "是否在售，0未售，1在售"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_spu", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_sku",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    spu_id = table.Column<long>(type: "bigint(20)", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false, comment: "商品名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    images = table.Column<string>(type: "varchar(1024)", nullable: true, comment: "商品的图片，多个图片以‘,’分割")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<long>(type: "bigint(15)", nullable: false, comment: "销售价格，单位为分"),
                    indexes = table.Column<string>(type: "varchar(32)", nullable: false, defaultValueSql: "''", comment: "sku规格在spu规格中的对应下标组合")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    spec = table.Column<string>(type: "varchar(1024)", nullable: false, defaultValueSql: "''", comment: "规格参数键值对，json格式")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sku", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_sku_tb_spu_spu_id",
                        column: x => x.spu_id,
                        principalTable: "tb_spu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_spu_detail",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false),
                    introduction = table.Column<string>(type: "text", nullable: false, comment: "商品介绍")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    spec = table.Column<string>(type: "varchar(2048)", nullable: false, comment: "商品规格")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    parameter = table.Column<string>(type: "varchar(2048)", nullable: false, comment: "商品参数")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_spu_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_spu_detail_tb_spu_id",
                        column: x => x.id,
                        principalTable: "tb_spu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sku_spu_id",
                table: "tb_sku",
                column: "spu_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_sku");

            migrationBuilder.DropTable(
                name: "tb_spu_detail");

            migrationBuilder.DropTable(
                name: "tb_spu");
        }
    }
}
