using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using Zhaoxi.MSACommerce.BrandService.Infrastructure.Data;

namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure.Data;


public sealed class BrandDesignTimeDbContextFactory : IDesignTimeDbContextFactory<BrandDbContext>
{
    /// <summary>
    /// 设计时创建 <see cref="BrandDbConnection"/>（供 EF 迁移/更新数据库使用，避免依赖启动项目的 Host/DI）。
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <returns><see cref="BrandDbConnection"/> 实例</returns>
    public BrandDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json",            optional: true)
                                          .AddJsonFile("appsettings.Development.json",optional: true)
                                          .AddEnvironmentVariables()
                                          .Build();

        string? dbConnection = configuration.GetConnectionString("BrandDbConnection");
        if (string.IsNullOrWhiteSpace(dbConnection))
        {
            throw new InvalidOperationException("连接字符串 ConnectionStrings:BrandDbConnection 为空，无法创建 BrandDbContext。");
        }

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
        DbContextOptions<BrandDbContext> options = new DbContextOptionsBuilder<BrandDbContext>()
                                                  .UseMySql(dbConnection, serverVersion)
                                                  .Options;
        return new BrandDbContext(options);
    }
}
