using Polly;
using Polly.Extensions.Http;

namespace Zhaoxi.MSACommerce.LoadBalancer.Policy;

/// <summary>
/// 熔断策略：当在一个时间段内，失败率达到70%，并且至少有5次请求失败，则触发熔断，熔断持续6秒
/// </summary>
public static class CircuitBreakerStrategy
{
    /// <summary>
    /// 熔断策略：当在一个时间段内，失败率达到70%，并且至少有5次请求失败，则触发熔断，熔断持续6秒
    /// </summary>
    /// <returns></returns>
    public static IAsyncPolicy<HttpResponseMessage> Build()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .AdvancedCircuitBreakerAsync(
                0.7, 
                TimeSpan.FromSeconds(3),
                5, 
                TimeSpan.FromSeconds(6));
    }
}