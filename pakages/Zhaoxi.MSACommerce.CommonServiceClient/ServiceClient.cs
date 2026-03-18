using Refit;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.LoadBalancer.AspNetCore;

namespace Zhaoxi.MSACommerce.LoadBalancer;

public class ServiceClient<TServiceApi> : IServiceClient<TServiceApi> where TServiceApi : class
{
    public string      ServiceName { get; set; }
    public TServiceApi ServiceApi  { get; set; }

    public ServiceClient(IServiceDiscovery          serviceDiscovery,
                         ILoadBalancer<TServiceApi> loadBalancer,
                         HttpClient                 httpClient)
    {
        ServiceName = loadBalancer.ServiceName;
        var serviceList    = serviceDiscovery.GetServicesAsync(ServiceName).Result;
        var serviceAddress = loadBalancer.GetNode(serviceList);

        httpClient.BaseAddress = new Uri($"http://{serviceAddress}");
        ServiceApi             = RestService.For<TServiceApi>(httpClient);
    }
}