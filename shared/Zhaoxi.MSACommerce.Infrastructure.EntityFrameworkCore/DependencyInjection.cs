using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Infrastructure.Common.Interceptors;

namespace Zhaoxi.MSACommerce.Infrastructure.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureCommon(this IServiceCollection services)
    {
        services.AddScoped<AuditEntityInterceptor>();

        return services;
    }
}
