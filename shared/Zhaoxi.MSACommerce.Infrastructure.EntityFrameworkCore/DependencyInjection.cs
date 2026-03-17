using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Infrastructure.Common.Interceptors;

namespace Zhaoxi.MSACommerce.Infrastructure.Common;

/// <summary>
/// 依赖注入 主要为了审计实体拦截器 后续需要添加其他基础设施相关的服务也可以放在这里
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEfCore(this IServiceCollection services)
    {
        services.AddScoped<AuditEntityInterceptor>();

        return services;
    }
}