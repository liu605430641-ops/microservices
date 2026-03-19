using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class 商品 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_category_brands_tb_category_category_id",
                table: "tb_category_brands");

            migrationBuilder.CreateTable(
                name: "CategoryCategoryBrands",
                columns: table => new
                {
                    BrandsId = table.Column<long>(type: "bigint(20)", nullable: false),
                    CategoriesId = table.Column<long>(type: "bigint(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCategoryBrands", x => new { x.BrandsId, x.CategoriesId });
                    table.ForeignKey(
                        name: "FK_CategoryCategoryBrands_tb_category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "tb_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCategoryBrands_tb_category_brands_BrandsId",
                        column: x => x.BrandsId,
                        principalTable: "tb_category_brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategoryBrands_CategoriesId",
                table: "CategoryCategoryBrands",
                column: "CategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCategoryBrands");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_category_brands_tb_category_category_id",
                table: "tb_category_brands",
                column: "category_id",
                principalTable: "tb_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
