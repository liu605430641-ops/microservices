using Zhaoxi.MSACommerce.LoadBalancer.Strategies;

namespace Zhaoxi.MSACommerce.LoadBalancer;

public class LoadBalancer(ServiceClientOption option) : ILoadBalancer
{
    private readonly ILoadBalancingStrategy _strategy = option.LoadBalancingStrategy switch
                                                        {
                                                            LoadBalancingStrategy.Random     => new RandomStrategy(),
                                                            LoadBalancingStrategy.RoundRobin => new RoundRobinStrategy(),
                                                            _                                => throw new ArgumentOutOfRangeException()
                                                        };

    public string ServiceName { get; set; } = option.ServiceName;

    public string GetNode(List<string> nodes)
    {
        return _strategy.Resolve(nodes);
    }
}