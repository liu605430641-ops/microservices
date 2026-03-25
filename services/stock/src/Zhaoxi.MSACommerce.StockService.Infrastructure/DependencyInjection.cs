using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Infrastructure.Common;
using Zhaoxi.MSACommerce.StockService.Infrastructure.Data;

namespace Zhaoxi.MSACommerce.StockService.Infrastructure;

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
        var dbConnection = configuration.GetConnectionString("StockDbConnection");

        services.AddDbContext<StockDbContext>((sp, options) =>
        {
            options.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection));
        });
    }
    
    private static void ConfigureCap(IServiceCollection services,  IConfiguration configuration)
    {
        var dbConn = configuration.GetConnectionString("StockDbConnection");
        if (dbConn is null) throw new ArgumentNullException(nameof(dbConn));
        
        services.AddCap(x =>
        {   
            x.UseEntityFramework<StockDbContext>();
            x.UseMySql(dbConn);
            x.UseRabbitMQ(options =>
            {
                configuration.GetSection("RabbitMQ").Bind(options);
            });
            x.UseDashboard();
        }); 
    }
}