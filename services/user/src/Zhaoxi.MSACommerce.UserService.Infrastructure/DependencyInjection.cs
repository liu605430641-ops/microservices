using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Infrastructure.Common;
using Zhaoxi.MSACommerce.Infrastructure.Common.Interceptors;
using Zhaoxi.MSACommerce.UserService.Infrastructure.Data;

namespace Zhaoxi.MSACommerce.UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureCommon(configuration);
        services.AddInfrastructureEfCore();
        
        ConfigureEfCore(services, configuration);

        return services;
    }

    private static void ConfigureEfCore(IServiceCollection services, IConfiguration configuration)
    {
        // 优先从配置读，读不到就用你刚才命令行里的那个地址
        var dbConnection = configuration.GetConnectionString("UserDbConnection") 
                           ?? "server=34.96.203.128;port=3306;userid=root;password=123123;database=zhaoxi_user";

        services.AddDbContext<UserDbContext>((sp, options) =>
                                             {
                                                 options.AddInterceptors(sp.GetRequiredService<AuditEntityInterceptor>());
        
                                                 // 建议手动指定版本，避免 AutoDetect 在读不到配置时直接崩溃
                                                 var serverVersion = new MySqlServerVersion(new Version(8, 0, 21)); 
                                                 options.UseMySql(dbConnection, serverVersion);
                                             });
    }
}
