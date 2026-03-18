using Refit; // Refit（类型安全的Http客户端库，通过接口生成HTTP调用代理）
using Zhaoxi.MSACommerce.AuthServer.Clients;
using Zhaoxi.MSACommerce.AuthServer.Services;
using Zhaoxi.MSACommerce.LoadBalancer;

namespace Zhaoxi.MSACommerce.AuthServer;

/// <summary>
/// 依赖注入统一入口（Composition Root（组合根））
/// 所有当前服务的依赖都在这里注册
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// 对外暴露的扩展方法（用于 Program.cs 调用）
    /// </summary>
    public static IServiceCollection AddHttpApi(this IServiceCollection services, IConfiguration configuration)
    {
        // 注册用户服务客户端（远程调用）
        ConfigureUserService(services, configuration);

        // 注册认证相关服务（JWT）
        ConfigureIdentity(services, configuration);

        return services;
    }

    /// <summary>
    /// 配置用户服务（UserService）的远程调用客户端
    /// </summary>
    private static void ConfigureUserService(IServiceCollection services,IConfiguration configuration)
    {
        services.AddServiceClient<UserServiceClient>(
            // ① 负载均衡策略配置
            options => 
            { 
                options.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin; // 轮询（RoundRobin（轮询））
            },
            // ② HttpClient配置
            Client => 
            { 
                Client.Timeout = TimeSpan.FromSeconds(1); // 超时时间1秒
            }
        );
    }

    /// <summary>
    /// 配置身份认证（JWT）
    /// </summary>
    private static void ConfigureIdentity(IServiceCollection services, IConfiguration configuration)
    {
        // ① 注册身份服务（单例）
        services.AddScoped<IIdentityService, IdentityService>();

        // ② 读取配置文件中的 JwtSettings
        var configurationSection = configuration.GetSection(nameof(JwtSettings));

        // 绑定配置到对象（Configuration Binding（配置绑定））
        var jwtSettings = configurationSection.Get<JwtSettings>();

        // 防御性编程（避免配置缺失）
        if (jwtSettings is null) 
            throw new NullReferenceException(nameof(jwtSettings));

        // ③ 注册配置到 IOptions（选项模式）
        services.Configure<JwtSettings>(configurationSection);
    }
}