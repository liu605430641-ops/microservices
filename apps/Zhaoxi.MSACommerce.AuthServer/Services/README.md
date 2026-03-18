## AuthServer 服务调用链路说明（IdentityService & UserServiceClient）

本文档说明 `IdentityService.GetAccessTokenAsync` 中，通过 `UserServiceClient` 调用用户服务的**完整调用链路**以及在 AuthServer 中需要做的**配置项**。

### 一、整体调用链路概览

- **入口**：`IdentityService.GetAccessTokenAsync(string username, string password)`
  - 在用户通过用户名/密码登录时被调用。
- **第一步：调用用户服务进行账号密码校验**
  - 代码：`await userServiceClient.UserServiceApi.GetUserAsync(username, password)`
  - 说明：通过 `UserServiceClient` 远程调用 UserService 的 HTTP 接口，验证用户名和密码是否正确。
- **第二步：根据用户信息生成 JWT Token**
  - 调用 `JwtSecurityToken`，使用配置的 `JwtSettings`（Issuer / Audience / Secret / 过期时间等）生成访问令牌。
- **返回结果**：`Result<string>`，成功时为 JWT 字符串，失败时返回错误信息（如“用户名或密码不正确”）。

### 二、UserServiceClient 与 CommonServiceClient 的关系

`UserServiceClient` 是一个**具体业务客户端实现**，基于通用组件 `Zhaoxi.MSACommerce.CommonServiceClient`（命名空间为 `Zhaoxi.MSACommerce.LoadBalancer`）进行封装：

- **构造函数依赖**
  - `IServiceDiscovery serviceDiscovery`：基于 Consul 的服务发现，实现从注册中心获取服务实例列表。
  - `ILoadBalancer<UserServiceClient> loadBalancer`：负载均衡器，从多个实例中选出一个可用节点。
  - `HttpClient httpClient`：真正发送 HTTP 请求的客户端。
- **继承关系**
  - `UserServiceClient : ServiceClient`
  - 父类 `ServiceClient` 会在构造函数中完成：
    1. 使用 `serviceDiscovery.GetServicesAsync(ServiceName)` 获取所有实例；
    2. 使用 `loadBalancer.GetNode(serviceList)` 选择一个实例；
    3. 将 `httpClient.BaseAddress` 设置为选中实例的地址（例如 `http://10.0.0.5:5001`）。
- **服务名称**
  - `public override string ServiceName { get; set; } = "Zhaoxi.MSACommerce.UserService.HttpApi";`
  - 会据此去 Consul 查找用户服务的实例列表。
- **Refit 接口代理**
  - `public readonly IUserService UserServiceApi = RestService.For<IUserService>(httpClient);`
  - 通过 Refit 把 `IUserService` 接口转成实际的 HTTP 客户端，`GetUserAsync` 等方法都会直接发 HTTP 请求。

> 总结：`IdentityService` 不直接操作 HTTP/Consul，只依赖 `UserServiceClient`；  
> `UserServiceClient` 再基于 CommonServiceClient 提供的服务发现 + 负载均衡能力，把远程调用封装为强类型方法。

### 三、配置项与依赖注入

#### 1. JWT 配置（AuthServer 本地配置）

`IdentityService` 通过 `IOptions<JwtSettings>` 读取 JWT 配置，通常在 `appsettings.json` / `appsettings.Development.json` 中配置，例如：

```json
"JwtSettings": {
  "Issuer": "Zhaoxi.MSACommerce.AuthServer",
  "Audience": "Zhaoxi.MSACommerce.WebClient",
  "Secret": "你的JWT密钥（足够长）",
  "AccessTokenExpirationMinutes": 60
}
```

并在 `DependencyInjection.cs` 或 `Program.cs` 中注册：

```csharp
services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
services.AddScoped<IIdentityService, IdentityService>();
```

#### 2. UserServiceClient 注入与 CommonServiceClient 相关配置

在 AuthServer 启动时，需要确保：

- **注册 Consul 服务发现**
  - 使用 `Zhaoxi.MSACommerce.Consul.ServiceDiscovery` 提供的扩展方法（例如在 CommonServiceClient 的 README 中提到的 `AddConsulDiscovery()`）。
- **注册负载均衡器与 ServiceClient**
  - 使用 CommonServiceClient 中的扩展方法（例如 `AddServiceClient<UserServiceClient>`）：

```csharp
services.AddServiceClient<UserServiceClient>(
    serviceClientOption =>
    {
        // 配置负载均衡策略：轮询 / 随机等
        serviceClientOption.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
    },
    httpClient =>
    {
        // 这里可以配置 HttpClient（超时、默认Header等）
        httpClient.Timeout = TimeSpan.FromSeconds(10);
    });
```

> 注意：`AddServiceClient<TServiceClient>` 内部会：
> - 注册 Consul 服务发现；
> - 注册负载均衡器；
> - 注册 `HttpClient<TServiceClient>` 和 `TServiceClient` 本身。

只要上述注册完成后，`IdentityService` 构造函数就可以直接注入 `UserServiceClient` 使用：

```csharp
public IdentityService(UserServiceClient userServiceClient, IOptions<JwtSettings> jwtSettings) { ... }
```

### 四、调用链路示意（从外到内）

1. **客户端（前端 / 网关）** 向 AuthServer 发送登录请求（用户名 + 密码）。
2. **AuthServer Controller** 调用 `IdentityService.GetAccessTokenAsync(username, password)`。
3. `IdentityService` 调用 `userServiceClient.UserServiceApi.GetUserAsync(username, password)`：
   - `UserServiceClient` 通过父类 `ServiceClient`：
     1. 向 Consul 查询 `Zhaoxi.MSACommerce.UserService.HttpApi` 的可用实例；
     2. 使用负载均衡策略选出一个实例；
     3. 使用配置好的 `HttpClient` 向该实例发起 HTTP 请求。
4. UserService 返回用户信息或认证失败。
5. 如果成功，`IdentityService` 使用用户信息构造 JWT，并返回给 Controller。
6. Controller 将 Token 返回给调用方，后续请求可携带该 Token 访问其他服务。

### 五、接入/排查建议

- **接入新服务时**
  - 参考 `UserServiceClient` 的写法，新增一个对应的 `XXXServiceClient`，继承 `ServiceClient` 并设置好 `ServiceName`；
  - 在启动时 `AddServiceClient<XXXServiceClient>`；
  - 在业务服务中通过构造函数注入 `XXXServiceClient` 使用。

- **排查调用问题时重点关注**
  - Consul 中是否有注册 `Zhaoxi.MSACommerce.UserService.HttpApi`；
  - 负载均衡策略是否合理（节点列表是否为空 / 选出的节点是否可用）；
  - `HttpClient` 的 BaseAddress 是否正确；
  - `JwtSettings` 配置是否正确（Issuer/Audience/Secret/过期时间）。

