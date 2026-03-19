using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure.Data;


public sealed class CategoryDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CategoryDbContext>
{
    /// <summary>
    /// 设计时创建 <see cref="CategoryDbContext"/>（供 EF 迁移/更新数据库使用，避免依赖启动项目的 Host/DI）。
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <returns><see cref="CategoryDbContext"/> 实例</returns>
    public CategoryDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json",            optional: true)
                                          .AddJsonFile("appsettings.Development.json",optional: true)
                                          .AddEnvironmentVariables()
                                          .Build();

        string? dbConnection = configuration.GetConnectionString("CategoryDbConnection");
        if (string.IsNullOrWhiteSpace(dbConnection))
        {
            throw new InvalidOperationException("连接字符串 ConnectionStrings:CategoryDbConnection 为空，无法创建 CategoryDbContext。");
        }

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
        DbContextOptions<CategoryDbContext> options = new DbContextOptionsBuilder<CategoryDbContext>()
                                                 .UseMySql(dbConnection, serverVersion)
                                                 .Options;
        return new CategoryDbContext(options);
    }
}
