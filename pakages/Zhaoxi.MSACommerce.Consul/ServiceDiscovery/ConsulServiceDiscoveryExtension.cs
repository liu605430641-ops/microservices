using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Zhaoxi.MSACommerce.Consul.ServiceDiscovery;

public static class ConsulServiceDiscoveryExtension
{
    public static IServiceCollection AddConsulDiscovery(this IServiceCollection services)
    {
        services.TryAddSingleton<IServiceDiscovery, ConsulServiceDiscovery>();
        return services;
    }
}
