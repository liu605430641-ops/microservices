using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Zhaoxi.MSACommerce.ProductService.Infrastructure.Data;


public sealed class ProductDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    /// <summary>
    /// 设计时创建 <see cref="ProductDbContext"/>（供 EF 迁移/更新数据库使用，避免依赖启动项目的 Host/DI）。
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <returns><see cref="ProductDbContext"/> 实例</returns>
    public ProductDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json",            optional: true)
                                          .AddJsonFile("appsettings.Development.json",optional: true)
                                          .AddEnvironmentVariables()
                                          .Build();

        string? dbConnection = configuration.GetConnectionString("ProductDbConnection");
        if (string.IsNullOrWhiteSpace(dbConnection))
        {
            throw new InvalidOperationException("连接字符串 ConnectionStrings:ProductDbConnection 为空，无法创建 ProductDbContext。");
        }

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
        DbContextOptions<ProductDbContext> options = new DbContextOptionsBuilder<ProductDbContext>()
                                                 .UseMySql(dbConnection, serverVersion)
                                                 .Options;
        return new ProductDbContext(options);
    }
}
