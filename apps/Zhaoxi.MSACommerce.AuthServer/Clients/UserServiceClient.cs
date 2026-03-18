using Refit; // Refit（类型安全HTTP客户端，通过接口生成代理）
using Zhaoxi.MSACommerce.AuthServer.Services;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery; // 服务发现（ServiceDiscovery（服务发现））
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.LoadBalancer.AspNetCore;

namespace Zhaoxi.MSACommerce.AuthServer.Clients;

/// <summary>
/// 用户服务客户端（封装对 UserService 的远程调用）
/// 本质：一个“带服务发现 + 负载均衡”的 HttpClient 封装
/// </summary>
public class UserServiceClient(
    IServiceDiscovery                serviceDiscovery, // ① 服务发现（从Consul获取服务实例列表）
    ILoadBalancer<UserServiceClient> loadBalancer,     // ② 负载均衡器（选择具体实例）
    HttpClient                       httpClient                     // ③ HTTP客户端（实际发请求）
)
    : ServiceClient(serviceDiscovery, loadBalancer, httpClient)    // 继承基础客户端（核心逻辑在父类）
{
    /// <summary>
    /// 服务名称（用于服务发现）
    /// 👉 会去 Consul 查这个名字对应的实例列表
    /// </summary>
    public override string ServiceName { get; set; } = "Zhaoxi.MSACommerce.UserService.HttpApi";

    /// <summary>
    /// Refit 生成的接口代理
    /// 👉 调用这个接口 = 发HTTP请求
    /// </summary>
    public readonly IUserService UserServiceApi = RestService.For<IUserService>(httpClient);
}