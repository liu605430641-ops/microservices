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
                name: "tb_param_group",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(32)", nullable: false, comment: "参数组名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    category_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "所属品类"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_param_group", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_spec_key",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "规格Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(256)", nullable: false, comment: "规格名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    category_Id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "所属分类"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_spec_key", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_category_brand",
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
                    table.PrimaryKey("PK_tb_category_brand", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_category_brand_tb_category_category_id",
                        column: x => x.category_id,
                        principalTable: "tb_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_param_key",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "参数Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(256)", nullable: false, comment: "参数名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    param_group_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "所属分组"),
                    category_Id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "所属分类"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_param_key", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_param_key_tb_param_group_param_group_id",
                        column: x => x.param_group_id,
                        principalTable: "tb_param_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_category_brand_category_id_brand_id",
                table: "tb_category_brand",
                columns: new[] { "category_id", "brand_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_param_group_category_id",
                table: "tb_param_group",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_param_key_category_Id",
                table: "tb_param_key",
                column: "category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_param_key_param_group_id",
                table: "tb_param_key",
                column: "param_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_spec_key_category_Id",
                table: "tb_spec_key",
                column: "category_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_category_brand");

            migrationBuilder.DropTable(
                name: "tb_param_key");

            migrationBuilder.DropTable(
                name: "tb_spec_key");

            migrationBuilder.DropTable(
                name: "tb_category");

            migrationBuilder.DropTable(
                name: "tb_param_group");
        }
    }
}
