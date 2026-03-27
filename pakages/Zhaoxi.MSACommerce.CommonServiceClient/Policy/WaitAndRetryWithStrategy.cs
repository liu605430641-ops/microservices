using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Zhaoxi.MSACommerce.LoadBalancer.Policy;

/// <summary>
/// polly的重试策略：当发生瞬时HTTP错误时，使用抖动退避算法进行重试，重试次数为5次，初始延迟为1秒，每次重试的延迟会根据抖动算法进行调整，以避免重试风暴。
/// </summary>
public static class WaitAndRetryWithStrategy
{
    /// <summary>
    /// 抖动重试策略：当发生瞬时HTTP错误时，使用抖动退避算法进行重试，重试次数为5次，初始延迟为1秒，每次重试的延迟会根据抖动算法进行调整，以避免重试风暴。
    /// </summary>
    /// <returns></returns>
    public static IAsyncPolicy<HttpResponseMessage> Build()
    {
        // 使用抖动退避算法生成重试延迟时间，初始延迟为1秒，重试次数为5次 时间为1s, 2s, 4s, 8s, 16s，但每次重试的延迟会根据抖动算法进行调整，以避免重试风暴。
        var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5);
        
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(delay);
    }
}