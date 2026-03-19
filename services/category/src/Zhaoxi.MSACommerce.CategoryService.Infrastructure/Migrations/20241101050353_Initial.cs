using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure.Migrations
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
                name: "tb_category",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "品类id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(32)", nullable: false, comment: "品类名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    parent_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "父类id,顶级类填0"),
                    is_parent = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否为父节点，0为否，1为是"),
                    sort = table.Column<int>(type: "int(4)", nullable: false, comment: "排序指数，越小越靠前"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_category", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_category_brands",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    category_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "品类id"),
                    brand_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "品牌id"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_category_brands", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_category_brands_tb_category_category_id",
                        column: x => x.category_id,
                        principalTable: "tb_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_category_brands_category_id_brand_id",
                table: "tb_category_brands",
                columns: new[] { "category_id", "brand_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_category_brands");

            migrationBuilder.DropTable(
                name: "tb_category");
        }
    }
}
