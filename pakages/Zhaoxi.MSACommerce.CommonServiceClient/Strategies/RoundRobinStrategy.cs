namespace Zhaoxi.MSACommerce.LoadBalancer.Strategies;

public class RoundRobinStrategy : ILoadBalancingStrategy
{
    private int _index;

    public string Resolve(List<string> nodes)
    {
        if (nodes.Count == 0)
        {
            throw new InvalidOperationException("无可用节点");
        }

        _index = Interlocked.Increment(ref _index) % nodes.Count;
        return nodes[_index];
    }
}
