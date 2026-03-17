using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.LoadBalancer.AspNetCore;

namespace Zhaoxi.MSACommerce.LoadBalancer;

public static class ServiceClientExtension
{
    public static void AddServiceClient<TServiceClient>(this IServiceCollection services,
        Action<ServiceClientOption> configureServiceClient,
        Action<HttpClient> configureHttpClient)
        where TServiceClient : class, ISeviceClient
    {
        var serviceClientOption = new ServiceClientOption();
        configureServiceClient.Invoke(serviceClientOption);

        services.AddConsulDiscovery();

        services.AddLoadBalancer<TServiceClient>(serviceClientOption.LoadBalancingStrategy);

        services.AddHttpClient<TServiceClient>(configureHttpClient);

        services.AddScoped<TServiceClient>();
    }
}
