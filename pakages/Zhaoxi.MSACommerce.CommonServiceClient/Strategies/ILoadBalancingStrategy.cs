namespace Zhaoxi.MSACommerce.LoadBalancer.Strategies;

public interface ILoadBalancingStrategy
{
    string Resolve(List<string> nodes);
}
