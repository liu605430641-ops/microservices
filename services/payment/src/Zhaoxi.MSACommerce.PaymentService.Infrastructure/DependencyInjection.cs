using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Infrastructure.Common;
using Zhaoxi.MSACommerce.Infrastructure.Common.Interceptors;
using Zhaoxi.MSACommerce.PaymentService.Infrastructure.Data;

namespace Zhaoxi.MSACommerce.PaymentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureCommon(configuration);

        services.AddInfrastructureEfCore();

        ConfigureEfCore(services, configuration);
        
        ConfigureCap(services, configuration);
        
        return services;
    }

    private static void ConfigureEfCore(IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("PaymentDbConnection");

        services.AddDbContext<PaymentDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<AuditEntityInterceptor>());
            options.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection));
        });
    }
    
    private static void ConfigureCap(IServiceCollection services,  IConfiguration configuration)
    {
        var dbConn = configuration.GetConnectionString("PaymentDbConnection");
        if (dbConn is null) throw new ArgumentNullException(nameof(dbConn));
        
        services.AddCap(x =>
        {   
            x.UseEntityFramework<PaymentDbContext>();
            x.UseMySql(dbConn);
            x.UseRabbitMQ(options =>
            {
                configuration.GetSection("RabbitMQ").Bind(options);
            });
            x.UseDashboard();
        }); 
    }
}