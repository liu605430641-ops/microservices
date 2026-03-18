using StackExchange.Redis;

namespace Zhaoxi.MSACommerce.VerificationServer;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureRedis(services, configuration);

        ConfigureCors(services);

        return services;
    }

    private static void ConfigureRedis(IServiceCollection services, IConfiguration configuration)
    {
        var redisConn = configuration.GetConnectionString("RedisConnection");
        if (redisConn != null)
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));
    }

    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}
