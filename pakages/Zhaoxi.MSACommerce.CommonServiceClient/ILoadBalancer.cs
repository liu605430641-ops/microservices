namespace Zhaoxi.MSACommerce.LoadBalancer;

public interface ILoadBalancer
{
    string GetNode(List<string> nodes);
}
