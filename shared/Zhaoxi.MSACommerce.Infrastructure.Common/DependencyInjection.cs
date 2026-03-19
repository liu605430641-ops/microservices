using Consul.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.Consul.ServiceRegistration;

namespace Zhaoxi.MSACommerce.Infrastructure.Common;

public static class DependencyInjection
{
    /// <summary>
    /// 注册基础设施公共能力（如服务注册/发现等）。
    /// </summary>
    /// <param name="services">依赖注入容器</param>
    /// <param name="configuration">应用配置</param>
    /// <returns>依赖注入容器</returns>
    public static IServiceCollection AddInfrastructureCommon(this IServiceCollection services,
                                                             IConfiguration          configuration)
    {
        ConfigureConsul(services, configuration);

        return services;
    }

    /// <summary>
    /// 配置 Consul 服务注册/发现。
    /// </summary>
    /// <param name="services">依赖注入容器</param>
    /// <param name="configuration">应用配置</param>
    private static void ConfigureConsul(IServiceCollection services, IConfiguration configuration)
    {
        var applicationUrl = configuration["urls"] ?? configuration["applicationUrl"];
        if (string.IsNullOrWhiteSpace(applicationUrl))
        {
            // EF Core 设计时（迁移/更新数据库）可能不会提供 urls/applicationUrl；
            // 此场景下不需要服务注册，直接跳过以避免 new Uri(null) 崩溃。
            return;
        }

        var configurationSection = configuration.GetSection("ServiceCheck");
        var serviceCheck         = configurationSection.Get<ServiceCheckConfiguration>();
        services.Configure<ServiceConfiguration>(configurationSection);

        services.AddConsul();
        services.AddConsulService(serviceConfiguration =>
                                  {
                                      serviceConfiguration.ServiceAddress = new Uri(applicationUrl);
                                  }, serviceCheck);

        services.AddConsulDiscovery();
    }
}