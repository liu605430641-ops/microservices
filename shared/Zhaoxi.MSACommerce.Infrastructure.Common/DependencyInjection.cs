using Consul.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.Consul.ServiceRegistration;

namespace Zhaoxi.MSACommerce.Infrastructure.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureCommon(this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureConsul(services, configuration);
        return services;
    }

    private static void ConfigureConsul(IServiceCollection services, IConfiguration configuration)
    {
        services.AddConsul();
        
        var configurationSection = configuration.GetSection("ServiceCheck");
        var serviceCheck = configurationSection.Get<ServiceCheckConfiguration>();
        services.Configure<ServiceConfiguration>(configurationSection);
        
        var serviceAddress = configuration["urls"] ?? configuration["applicationUrl"];
        if (string.IsNullOrEmpty(serviceAddress)) return;
        
        services.AddConsulService(serviceConfiguration =>
        {
            serviceConfiguration.ServiceAddress = new Uri(configuration["urls"] ?? configuration["applicationUrl"]);
        }, serviceCheck);
    }
}
