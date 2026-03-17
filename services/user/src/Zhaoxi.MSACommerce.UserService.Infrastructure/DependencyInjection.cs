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
        var dbConnection = configuration.GetConnectionString("UserDbConnection");

        services.AddDbContext<UserDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<AuditEntityInterceptor>());
            options.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection));
        });
    }
}
