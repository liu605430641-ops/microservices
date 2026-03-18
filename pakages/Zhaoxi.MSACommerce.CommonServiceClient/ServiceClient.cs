using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;

namespace Zhaoxi.MSACommerce.LoadBalancer;

/// <summary>
/// 一句话总结
///ServiceClient 把“节点列表”交给 ILoadBalancer.GetNode，而 LoadBalancer.GetNode 再交给不同策略的 Resolve 去“选实例”，这整条链路就是负载均衡能力的体现。
/// </summary>
public abstract class ServiceClient : ISeviceClient
{
    public virtual string ServiceName { get; set; }

    /// <summary>
    /// 一句话总结
    ///  ServiceClient 把“节点列表”交给 ILoadBalancer.GetNode，而 LoadBalancer.GetNode 再交给不同策略的 Resolve 去“选实例”，这整条链路就是负载均衡能力的体现。
    /// </summary>
    /// <param name="serviceDiscovery"></param>
    /// <param name="loadBalancer"></param>
    /// <param name="httpClient"></param>
    protected ServiceClient(IServiceDiscovery serviceDiscovery,
        ILoadBalancer loadBalancer,
        HttpClient httpClient)
    {
        //从 Consul 获取服务实例列表
        var serviceList = serviceDiscovery.GetServicesAsync(ServiceName).Result;
        //这里把 Consul 返回的实例列表交给负载均衡器挑一个节点，并把 HttpClient.BaseAddress 指向该节点，这一步就是“负载均衡真正影响请求落到哪个实例”的地方。
        var serviceAddress = loadBalancer.GetNode(serviceList);

        httpClient.BaseAddress = new Uri($"http://{serviceAddress}");
    }


}
