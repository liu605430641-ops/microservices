using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;

namespace Zhaoxi.MSACommerce.LoadBalancer;

public abstract class ServiceClient : ISeviceClient
{
    public virtual string ServiceName { get; set; }

    protected ServiceClient(IServiceDiscovery serviceDiscovery,
        ILoadBalancer loadBalancer,
        HttpClient httpClient)
    {
        var serviceList = serviceDiscovery.GetServicesAsync(ServiceName).Result;
        var serviceAddress = loadBalancer.GetNode(serviceList);

        httpClient.BaseAddress = new Uri($"http://{serviceAddress}");
    }


}
