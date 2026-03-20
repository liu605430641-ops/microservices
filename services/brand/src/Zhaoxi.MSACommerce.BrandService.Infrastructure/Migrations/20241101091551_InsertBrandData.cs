using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhaoxi.MSACommerce.BrandService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertBrandData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assemblyDirectory= Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var sqlFilePath = Path.Combine(assemblyDirectory, "Migrations", "Scripts", "tb_brand.sql");
            var sql = File.ReadAllText(sqlFilePath);
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("truncate table tb_brand");
        }
    }
}
