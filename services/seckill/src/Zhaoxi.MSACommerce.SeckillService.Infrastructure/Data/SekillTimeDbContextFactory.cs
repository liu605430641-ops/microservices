using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Zhaoxi.MSACommerce.SeckillService.Infrastructure.Data;

namespace Zhaoxi.MSACommerce.SeckillService.Infrastructure.Data;


public sealed class SekillTimeDbContextFactory : IDesignTimeDbContextFactory<SecKillDbContext>
{
    /// <summary>
    /// 设计时创建 <see cref="SeckillDbConnection"/>（供 EF 迁移/更新数据库使用，避免依赖启动项目的 Host/DI）。
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <returns><see cref="SeckillDbConnection"/> 实例</returns>
    public SecKillDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json",            optional: true)
                                          .AddJsonFile("appsettings.Development.json",optional: true)
                                          .AddEnvironmentVariables()
                                          .Build();

        string? dbConnection = configuration.GetConnectionString("SeckillDbConnection");
        if (string.IsNullOrWhiteSpace(dbConnection))
        {
            throw new InvalidOperationException("连接字符串 ConnectionStrings:SeckillDbConnection 为空，无法创建 SeckillDbConnection。");
        }

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
        DbContextOptions<SecKillDbContext> options = new DbContextOptionsBuilder<SecKillDbContext>()
                                                    .UseMySql(dbConnection, serverVersion)
                                                    .Options;
        return new SecKillDbContext(options);
    }
}
