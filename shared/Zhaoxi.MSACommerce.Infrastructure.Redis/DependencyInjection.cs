using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Zhaoxi.MSACommerce.Infrastructure.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureRedis(services, configuration);

        return services;
    }
    

    private static void ConfigureRedis(IServiceCollection services, IConfiguration configuration)
    {
        var redisConn = configuration.GetConnectionString("RedisConnection");
        if (string.IsNullOrEmpty(redisConn)) throw new ArgumentNullException("RedisConnection");
        
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));
        services.AddScoped<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
    }
}
