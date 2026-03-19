using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace Zhaoxi.MSACommerce.UserService.Infrastructure.Data;

public sealed class UserDesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    /// <summary>
    /// 设计时创建 <see cref="UserDbContext"/>（供 EF 迁移/更新数据库使用，避免依赖启动项目的 Host/DI）。
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <returns><see cref="UserDbContext"/> 实例</returns>
    public UserDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                                           .SetBasePath(Directory.GetCurrentDirectory())
                                           .AddJsonFile("appsettings.json", optional: true)
                                           .AddJsonFile("appsettings.Development.json", optional: true)
                                           .AddEnvironmentVariables()
                                           .Build();

        string? dbConnection = configuration.GetConnectionString("UserDbConnection");
        if (string.IsNullOrWhiteSpace(dbConnection))
        {
            throw new InvalidOperationException("连接字符串 ConnectionStrings:UserDbConnection 为空，无法创建 UserDbContext。");
        }

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
        DbContextOptions<UserDbContext> options = new DbContextOptionsBuilder<UserDbContext>()
                                                  .UseMySql(dbConnection, serverVersion)
                                                  .Options;
        return new UserDbContext(options);
    }
}

