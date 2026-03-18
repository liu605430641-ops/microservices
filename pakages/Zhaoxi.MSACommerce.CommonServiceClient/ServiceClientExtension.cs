using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.LoadBalancer.AspNetCore;

namespace Zhaoxi.MSACommerce.LoadBalancer;

public static class ServiceClientExtension
{
    public static void AddServiceClient<TServiceApi>(this IServiceCollection     services,
                                                     Action<ServiceClientOption> configureServiceClient,
                                                     Action<HttpClient>          configureHttpClient)
        where TServiceApi : class
    {
        var serviceClientOption = new ServiceClientOption();
        configureServiceClient.Invoke(serviceClientOption);

        services.AddConsulDiscovery();

        services.AddLoadBalancer<TServiceApi>(serviceClientOption);

        services.AddHttpClient<TServiceApi>(configureHttpClient);

        services.AddScoped<IServiceClient<TServiceApi>, ServiceClient<TServiceApi>>();
    }
}