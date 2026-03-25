using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Zhaoxi.MSACommerce.PaymentService.Infrastructure.Data;

public sealed class paymentDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    /// <summary>
    /// 设计时创建 <see cref="PaymentDbContext"/>（供 EF 迁移/更新数据库使用，避免依赖启动项目的 Host/DI）。
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <returns><see cref="PaymentDbContext"/> 实例</returns>
    public PaymentDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json",            optional: true)
                                          .AddJsonFile("appsettings.Development.json",optional: true)
                                          .AddEnvironmentVariables()
                                          .Build();

        string? dbConnection = configuration.GetConnectionString("PaymentDbConnection");

        if (string.IsNullOrWhiteSpace(dbConnection))
        {
            throw new InvalidOperationException("连接字符串 ConnectionStrings:PaymentDbConnection 为空，无法创建 PaymentDbConnection。");
        }

        var serverVersion = new MySqlServerVersion(new Version(8,0,21));

        DbContextOptions<PaymentDbContext> options = new DbContextOptionsBuilder<PaymentDbContext>()
                                                    .UseMySql(dbConnection,serverVersion)
                                                    .Options;

        return new PaymentDbContext(options);
    }
}