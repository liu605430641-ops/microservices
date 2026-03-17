namespace Zhaoxi.MSACommerce.LoadBalancer.AspNetCore;

public class LoadBalancer<T>(LoadBalancingStrategy strategy) 
    : LoadBalancer(strategy), ILoadBalancer<T> where T : class;
