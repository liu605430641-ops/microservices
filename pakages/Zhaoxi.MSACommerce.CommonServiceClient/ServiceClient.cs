using Microsoft.Extensions.Logging;
using Refit;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.LoadBalancer.AspNetCore;

namespace Zhaoxi.MSACommerce.LoadBalancer;

public class ServiceClient<TServiceApi> : IServiceClient<TServiceApi> where TServiceApi : class
{
    public required string      ServiceName { get; set; }
    public required TServiceApi ServiceApi  { get; set; }

    public ServiceClient(IServiceDiscovery          serviceDiscovery,
                         ILoadBalancer<TServiceApi> loadBalancer,
                         HttpClient                 httpClient)
    {
        try
        {
            ServiceName = loadBalancer.ServiceName;
            var serviceList    = serviceDiscovery.GetServicesAsync(ServiceName).Result;
            var serviceAddress = loadBalancer.GetNode(serviceList);

            httpClient.BaseAddress = new Uri($"http://{serviceAddress}");
            ServiceApi             = RestService.For<TServiceApi>(httpClient);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[{ServiceName}]{e.Message}");
        }
        
    }
}