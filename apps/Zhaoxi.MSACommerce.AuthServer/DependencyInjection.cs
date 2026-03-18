using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.AuthServer.Apis;
using Zhaoxi.MSACommerce.AuthServer.Services;
using Zhaoxi.MSACommerce.LoadBalancer;

namespace Zhaoxi.MSACommerce.AuthServer;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureSwagger(services);
        
        ConfigureRedis(services, configuration);
        
        ConfigureUserService(services, configuration);

        ConfigureIdentity(services, configuration);

        ConfigureCors(services);

        return services;
    }
    
    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "鉴权中心",
                Version = "v1",
                Description = "一个微服务架构的电商平台实战项目"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "JWT Bearer 认证",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    private static void ConfigureRedis(IServiceCollection services, IConfiguration configuration)
    {
        var redisConn = configuration.GetConnectionString("RedisConnection");
        if (redisConn != null)
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));
        
    }

    private static void ConfigureUserService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceClient<IUserServiceApi>(option =>
        {
            option.ServiceName = "Zhaoxi.MSACommerce.UserService.HttpApi";
            option.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
        }, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(1);
        });
    }

    private static void ConfigureIdentity(IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ITokenService, TokenService>();

        // 从配置文件中读取JwtSettings，并注入到容器中
        var configurationSection = configuration.GetSection(nameof(JwtSettings));
        var jwtSettings = configurationSection.Get<JwtSettings>();
        if (jwtSettings is null) throw new NullReferenceException(nameof(jwtSettings));
        services.Configure<JwtSettings>(configurationSection);
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
