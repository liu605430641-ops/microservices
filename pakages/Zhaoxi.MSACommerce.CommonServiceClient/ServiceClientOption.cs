namespace Zhaoxi.MSACommerce.LoadBalancer;

public class ServiceClientOption
{
    public LoadBalancingStrategy LoadBalancingStrategy { get; set; } = LoadBalancingStrategy.RoundRobin;

}
