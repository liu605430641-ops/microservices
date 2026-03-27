using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Infrastructure.Common;
using Zhaoxi.MSACommerce.Infrastructure.Common.Interceptors;
using Zhaoxi.MSACommerce.Infrastructure.Redis;
using Zhaoxi.MSACommerce.SeckillService.Infrastructure.Data;

namespace Zhaoxi.MSACommerce.SeckillService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureCommon(configuration);
        
        services.AddInfrastructureRedis(configuration);

        ConfigureEfCore(services, configuration);

        return services;
    }
    
    private static void ConfigureEfCore(IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("SecKillDbConnection");

        services.AddDbContext<SecKillDbContext>((sp, options) =>
        {
            options.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection));
        });
    }
}