using IdGen.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Infrastructure.Common;
using Zhaoxi.MSACommerce.Infrastructure.Common.Interceptors;
using Zhaoxi.MSACommerce.OrderService.Infrastructure.Data;

namespace Zhaoxi.MSACommerce.OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureCommon(configuration);

        services.AddInfrastructureEfCore();

        ConfigureEfCore(services, configuration);

        ConfigureCap(services, configuration);
        
        //注册雪花id生成器，参数0表示workerId，单机部署时可设置为0，分布式部署时每个实例需设置不同的workerId（范围0-1023），以保证生成的ID全局唯一。
        //后续通过主机id分布式的时候动态获取
        services.AddIdGen(0);
        
        return services;
    }

    private static void ConfigureEfCore(IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("OrderDbConnection");

        services.AddDbContext<OrderDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<AuditEntityInterceptor>());
            options.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection));
        });
    }

    private static void ConfigureCap(IServiceCollection services,  IConfiguration configuration)
    {
        var dbConn = configuration.GetConnectionString("OrderDbConnection");
        if (dbConn is null) throw new ArgumentNullException(nameof(dbConn));
        
        services.AddCap(x =>
        {   
            x.UseEntityFramework<OrderDbContext>();
            x.UseMySql(dbConn);
            x.UseRabbitMQ(options =>
            {
                configuration.GetSection("RabbitMQ").Bind(options);
            });
            x.UseDashboard();
        }); 
    }
}