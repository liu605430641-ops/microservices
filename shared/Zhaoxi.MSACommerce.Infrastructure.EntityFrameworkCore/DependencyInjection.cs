using Consul.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Consul.ServiceRegistration;
using Zhaoxi.MSACommerce.Infrastructure.Common.Interceptors;

namespace Zhaoxi.MSACommerce.Infrastructure.Common;

/// <summary>
/// 基础设施层（Infrastructure）通用依赖注入扩展类
/// 
/// 设计目的：
/// 1. 统一管理基础设施相关的注册逻辑（如 Consul、拦截器等）
/// 2. 对外提供扩展方法，供 Program.cs 或 Startup 调用
/// 
/// 本质：
/// 是一个 DI（依赖注入）注册聚合入口
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// 对外暴露的扩展方法（推荐调用入口）
    /// 
    /// 作用：
    /// 将“基础设施层通用能力”统一注册到 IOC 容器中
    /// 
    /// 使用方式：
    /// builder.Services.AddInfrastructureCommon(configuration);
    /// 
    /// 设计思想：
    /// ✔ 模块化注册（Module Registration）
    /// ✔ 解耦 Program.cs（避免注册逻辑散乱）
    /// </summary>
    public static IServiceCollection AddInfrastructureCommon(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 注册 Consul 服务注册与健康检查
        ConfigureConsul(services, configuration);

        return services;
    }


    /// <summary>
    /// Consul 配置方法
    /// 
    /// 核心职责：
    /// 1. 读取配置文件中的 Consul 配置
    /// 2. 注册 Consul Client
    /// 3. 注册当前服务到 Consul（服务注册）
    /// 4. 配置健康检查（Health Check）
    /// 
    /// 背后机制（重点）：
    /// 服务启动 → 调用 Consul API → 注册自身信息
    /// Consul 保存服务列表 → 供网关或其他服务发现
    /// </summary>
    private static void ConfigureConsul(
        IServiceCollection services,
        IConfiguration configuration)
    {
        // =============================
        // 1. 读取配置节点（ServiceCheck）
        // =============================
        // 从 appsettings.json 中读取：
        // "ServiceCheck": { ... }
        var configurationSection = configuration.GetSection("ServiceCheck");

        // =============================
        // 2. 绑定配置到强类型对象
        // =============================
        // ConfigurationBinder.Get<T>：
        // 把配置转换为 C# 对象（强类型配置）
        var serviceCheck = ConfigurationBinder
            .Get<ServiceCheckConfiguration>(configurationSection);

        // =============================
        // 3. 注册配置（Options模式）
        // =============================
        // services.Configure<T>：
        // 将配置注册到 IOC，供 IOptions<T> 使用
        services.Configure<ServiceConfiguration>(configurationSection);

        // =============================
        // 4. 注册 Consul 客户端
        // =============================
        // AddConsul()：
        // 本质是注册 IConsulClient（用于调用 Consul API）
        services.AddConsul();

        // =============================
        // 5. 注册当前服务到 Consul
        // =============================
        services.AddConsulService(
            serviceConfiguration =>
            {
                // 设置当前服务地址（非常关键）
                // Consul 会用这个地址进行服务发现

                // 优先读取 urls（Kestrel配置）
                // fallback 到 applicationUrl
                serviceConfiguration.ServiceAddress =
                    new Uri(configuration["urls"] ?? configuration["applicationUrl"]);
            },
            serviceCheck // 健康检查配置
        );
    }
}