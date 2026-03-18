namespace Zhaoxi.MSACommerce.LoadBalancer.Strategies;

/// <summary>
/// 具体负载均衡算法：轮询 / 随机策略的 Resolve
/// 随机（Random）：随机生成索引选节点。
/// 轮询（RoundRobin）：用 Interlocked.Increment 做线程安全自增，然后对节点数取模选节点。
/// </summary>
public class RandomStrategy : ILoadBalancingStrategy
{
    private readonly Random _random = new();

    public string Resolve(List<string> nodes)
    {
        if (nodes.Count == 0)
        {
            throw new InvalidOperationException("无可用节点");
        }
        var index = _random.Next(nodes.Count);
        return nodes[index];
    }
}
