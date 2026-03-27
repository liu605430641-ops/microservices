using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.LoadBalancer.AspNetCore;
using Zhaoxi.MSACommerce.LoadBalancer.Policy;

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

        //调整HttpClient的生命周期默认为2分钟调整为5分钟和添加Polly的重试和熔断策略
        services.AddHttpClient<TServiceApi>(configureHttpClient)
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(WaitAndRetryWithStrategy.Build())
                .AddPolicyHandler(CircuitBreakerStrategy.Build());

        
        services.AddScoped<IServiceClient<TServiceApi>, ServiceClient<TServiceApi>>();
    }
}