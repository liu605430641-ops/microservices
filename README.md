# Zhaoxi.MSACommerce.CartService (购物车微服务)

这是一个基于 **.NET Core (C#)** 平台构建的购物车微服务项目。本项目采用了**清晰架构（Clean Architecture / 洋葱架构）**进行分层设计，将业务逻辑与基础设施、UI（API）严格解耦，提高代码的可维护性、可测试性和扩展性。

## 技术栈与框架
* **运行环境**: .NET Core / .NET (C#)
* **Web 框架**: ASP.NET Core Web API
* **数据存储**: Redis (主要用于购物车的临时高频数据存储)
* **依赖注入**: Microsoft.Extensions.DependencyInjection (自带 DI 容器)

---

## 解决方案分层架构说明

本项目主要分为 4 个层级，依赖关系始终是**外层依赖内层**，核心领域层（Core）不依赖任何其他层。

### 1. Core 层 (`Zhaoxi.MSACommerce.CartService.Core`)
**定位**: 领域层 / 核心层 (Domain Layer)  
**作用**: 存放系统中最核心的业务模型和抽象接口。这一层不依赖任何具体的技术实现（如数据库框架、外部服务API等）。

**这里应该加什么**:
* **Entities (实体)**: 购物车的领域模型，如 `CartItem`、`ShoppingCart` 等。
* **Value Objects (值对象)**: 领域驱动设计中的不可变对象。
* **Domain Exceptions (领域异常)**: 核心业务规则校验失败时抛出的异常。
* **Repository Interfaces (仓储接口)**: 例如 `ICartRepository`，定义对数据的操作接口，但不实现它（实现在 Infrastructure 层）。

---

### 2. UseCases 层 (`Zhaoxi.MSACommerce.CartService.UseCases`)
**定位**: 应用层 / 用例层 (Application Layer)  
**作用**: 编排领域对象以实现具体的业务用例（Business Use Cases）。它依赖于 Core 层。

**这里应该加什么**:
* **Commands & Queries (命令与查询)**: 比如 `AddCartItemCommand`, `GetCartQuery` 等（如果使用 CQRS 模式）。
* **Services / Handlers (应用服务或处理程序)**: 具体的业务编排逻辑。例如，“添加商品到购物车”的业务流程：调用接口校验库存 -> 组装 CartItem -> 调用仓储接口保存。
* **DTOs (数据传输对象)**: 用于在 API 层与应用层之间传递数据，比如请求模型和响应模型。
* **Validator (验证器)**: 比如使用 FluentValidation 对输入参数进行校验。

---

### 3. Infrastructure 层 (`Zhaoxi.MSACommerce.CartService.Infrastructure`)
**定位**: 基础设施层 (Infrastructure Layer)  
**作用**: 提供对 Core 层抽象接口的具体实现。处理一切涉及外部系统交互的技术细节（数据库、缓存、消息队列等）。它依赖 Core 层和 UseCases 层。

**这里应该加什么**:
* **Data Access (数据访问实现)**: 实现 Core 层定义的仓储接口，例如基于 Redis 的 `RedisCartRepository`。
* **External Service Clients (外部服务调用)**: 如果购物车服务需要调用商品服务(Product Service)或者促销服务获取信息，封装的 HTTP/gRPC 客户端放在这里。
* **Message Broker (消息队列)**: RabbitMQ / Kafka / CAP 的发布者和订阅者的底层集成。
* **IoC 注册扩展**: 比如 `DependencyInjection.cs` 中的 `AddInfrastructure()` 方法，将该层的实现注册到依赖注入容器中。

---

### 4. HttpApi 层 (`Zhaoxi.MSACommerce.CartService.HttpApi`)
**定位**: 表现层 / Web API 层 (Presentation Layer)  
**作用**: 作为整个微服务的入口点（Entry Point），对外暴露 HTTP RESTful 接口或 gRPC 终结点。它只负责接收请求、解析参数、调用应用层（UseCases），然后格式化返回结果。

**这里应该加什么**:
* **Controllers (控制器)**: 包含诸如 `CartController`，对外提供 `GET /api/cart`, `POST /api/cart/items` 等路由接口。
* **Middlewares (中间件)**: 统一异常处理、日志记录、限流、鉴权等全局过滤拦截组件。
* **配置文件 (appsettings.json)**: 包含 Redis 连接字符串、环境变量配置等。
* **Program.cs / Startup**: 应用程序启动配置、服务注册整合（调用内层和基础设施层的 `AddServices()` 扩展方法）。

---

## 开发规范提示
1. **不要跨层调用**: `HttpApi` 不能直接绕过 `UseCases` 去写 `Core` 层复杂的业务，也不应该在 `HttpApi` 层写 SQL 或缓存操作代码。
2. **依赖倒置原则 (DIP)**: `Infrastructure` 中的类应该实现 `Core` 层的接口。在 `HttpApi` 的依赖注入中绑定它们，从而让业务层只需依赖接口，不需要依赖 Redis 的具体实现包。
3. **面向接口编程**: 在 `UseCases` 层注入和使用的必须是接口（如 `ICartRepository`），绝对不可以直接实例化或注入 `RedisCartRepository`。