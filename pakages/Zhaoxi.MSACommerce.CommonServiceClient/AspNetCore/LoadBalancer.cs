namespace Zhaoxi.MSACommerce.LoadBalancer.AspNetCore;

public class LoadBalancer<T>(ServiceClientOption option) : LoadBalancer(option), ILoadBalancer<T> where T : class;