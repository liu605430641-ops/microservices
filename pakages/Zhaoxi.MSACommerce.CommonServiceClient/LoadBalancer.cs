using Zhaoxi.MSACommerce.LoadBalancer.Strategies;

namespace Zhaoxi.MSACommerce.LoadBalancer;


/// <summary>
/// 负载均衡器本体：LoadBalancer.GetNode 将选择交给具体策略
/// LoadBalancer 根据枚举 LoadBalancingStrategy 选择一个策略实现（随机/轮询），然后 GetNode 调用 _strategy.Resolve(nodes) 返回被选中的节点。
/// </summary>
/// <param name="strategy"></param>
public class LoadBalancer(LoadBalancingStrategy strategy) : ILoadBalancer
{
    private readonly ILoadBalancingStrategy _strategy = strategy switch
    {
        LoadBalancingStrategy.Random => new RandomStrategy(),
        LoadBalancingStrategy.RoundRobin => new RoundRobinStrategy(),
        _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null)
    };
    public string GetNode(List<string> nodes)
    {
        return _strategy.Resolve(nodes);
    }
}
