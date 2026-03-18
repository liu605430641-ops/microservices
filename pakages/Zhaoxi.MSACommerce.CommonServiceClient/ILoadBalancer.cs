namespace Zhaoxi.MSACommerce.LoadBalancer;

public interface ILoadBalancer
{
    string ServiceName { get; set; }
    
    string GetNode(List<string> nodes);
}