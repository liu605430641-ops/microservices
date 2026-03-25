using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Zhaoxi.MSACommerce.StockService.Infrastructure.Data;

namespace Zhaoxi.MSACommerce.StockService.Infrastructure.Data;

public sealed class StockDesignTimeDbContextFactory : IDesignTimeDbContextFactory<StockDbContext>
{
    /// <summary>
    /// 设计时创建 <see cref="StockDbConnection"/>（供 EF 迁移/更新数据库使用，避免依赖启动项目的 Host/DI）。
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <returns><see cref="StockDbConnection"/> 实例</returns>
    public StockDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                                           .SetBasePath(Directory.GetCurrentDirectory())
                                           .AddJsonFile("appsettings.json", optional: true)
                                           .AddJsonFile("appsettings.Development.json", optional: true)
                                           .AddEnvironmentVariables()
                                           .Build();

        string? dbConnection = configuration.GetConnectionString("StockDbConnection");
        if (string.IsNullOrWhiteSpace(dbConnection))
        {
            throw new InvalidOperationException("连接字符串 ConnectionStrings:StockDbContext 为空，无法创建 StockDbContext。");
        }

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
        DbContextOptions<StockDbContext> options = new DbContextOptionsBuilder<StockDbContext>()
                                                  .UseMySql(dbConnection, serverVersion)
                                                  .Options;
        return new StockDbContext(options);
    }
}

