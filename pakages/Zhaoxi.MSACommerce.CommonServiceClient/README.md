## Zhaoxi.MSACommerce.CommonServiceClient

`Zhaoxi.MSACommerce.CommonServiceClient` 是一个用于**微服务间 HTTP 调用的通用客户端封装组件**，基于 Consul 服务发现和可插拔负载均衡策略，为业务服务提供统一、规范、可扩展的服务调用能力。

### 核心功能

- **统一的服务客户端抽象**
  - 提供 `ISeviceClient` 接口与抽象基类 `ServiceClient`，业务侧只需继承后配置好 `ServiceName`，即可获得带服务发现和负载均衡能力的 HTTP 客户端。

- **内置服务发现能力**
  - 集成 `Zhaoxi.MSACommerce.Consul.ServiceDiscovery`，通过服务名从 Consul 拉取可用实例列表，而不是写死 IP/端口。

- **可配置负载均衡策略**
  - 通过 `LoadBalancingStrategy`（如 `RoundRobin`、`Random`）与对应策略实现类（`RoundRobinStrategy`、`RandomStrategy` 等），对下游服务实例进行流量分配。

- **与 ASP.NET Core 深度集成**
  - 通过扩展方法 `AddServiceClient<TServiceClient>` 和 `AddLoadBalancer<T>`，将服务发现、负载均衡和 `HttpClientFactory` 组合在一起，支持在依赖注入容器中直接注入强类型客户端。

### 典型使用场景

- 在用户服务、订单服务、商品服务等微服务中，通过继承 `ServiceClient` 封装对其他服务的调用逻辑。
- 不关心具体实例地址，只关注服务名，按需选择合适的负载均衡策略（轮询/随机等），提升可用性与扩展性。
- 搭配 WebGateway、AuthServer 等组件，实现端到端的服务注册、发现与访问。

### 关键概念说明

- **ISeviceClient**
  - 统一定义服务客户端的基本约束，尤其是 `ServiceName`，用于从 Consul 中查找对应服务实例。

- **ServiceClient**
  - 抽象基类，构造函数中依赖 `IServiceDiscovery`、`ILoadBalancer` 和 `HttpClient`。
  - 在构造过程中会：
    1. 通过服务发现获取指定 `ServiceName` 的实例列表。
    2. 借助负载均衡器从中选出一个实例。
    3. 将 `HttpClient.BaseAddress` 设置为选中实例的地址。

- **LoadBalancer / ILoadBalancer / ILoadBalancingStrategy**
  - 通过不同策略实现对可用节点列表进行选择，隐藏具体算法细节，对上层只暴露简单的 `GetNode` 接口。

- **ServiceClientExtension.AddServiceClient**
  - 组合注册：
    - 注册 Consul 服务发现：`AddConsulDiscovery()`；
    - 注册负载均衡：`AddLoadBalancer<TServiceClient>(strategy)`；
    - 注册 `HttpClient<TServiceClient>` 及 `TServiceClient` 本身。

### 简要使用示例（思路）

1. **定义具体业务客户端**
   - 新建一个类继承 `ServiceClient`，实现 `ISeviceClient`，在构造函数中设置 `ServiceName`。

2. **在应用启动时注册客户端**
   - 调用 `services.AddServiceClient<YourServiceClient>(...)`，配置负载均衡策略以及 `HttpClient`（超时、Header 等）。

3. **在业务代码中注入使用**
   - 直接通过构造函数注入 `YourServiceClient`，像调用本地服务一样发起 HTTP 请求，无需关注实例地址与负载均衡细节。

通过 `Zhaoxi.MSACommerce.CommonServiceClient`，整个微服务体系的服务调用逻辑可以被统一、标准化管理，降低重复造轮子和配置分散的风险，为后续扩展熔断、重试、监控等能力打下基础。

