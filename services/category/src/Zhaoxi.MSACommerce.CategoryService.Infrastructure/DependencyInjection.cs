using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.CategoryService.Infrastructure.Data;
using Zhaoxi.MSACommerce.Infrastructure.Common;
using Zhaoxi.MSACommerce.Infrastructure.Common.Interceptors;
using ZiggyCreatures.Caching.Fusion;

namespace Zhaoxi.MSACommerce.CategoryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureCommon(configuration);

        services.AddInfrastructureEfCore();

        ConfigureEfCore(services, configuration);

        ConfigureCache(services, configuration);
        
        return services;
    }

    private static void ConfigureEfCore(IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("CategoryDbConnection");

        services.AddDbContext<CategoryDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<AuditEntityInterceptor>());
            options.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection));
        });
    }
    
    private static void ConfigureCache(IServiceCollection services, IConfiguration configuration)
    {
        var redisConn = configuration.GetConnectionString("RedisConnection");
        if (redisConn != null)
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));

        services.AddStackExchangeRedisCache(options => options.Configuration = redisConn);
        services.AddFusionCache()
            .WithSystemTextJsonSerializer()
            .WithDistributedCache(provider => provider.GetRequiredService<IDistributedCache>());
    }
}