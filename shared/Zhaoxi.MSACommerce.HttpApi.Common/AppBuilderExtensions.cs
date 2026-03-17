using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Zhaoxi.MSACommerce.Consul.ServiceRegistration;

namespace Zhaoxi.MSACommerce.HttpApi.Common;

/// <summary>
/// HTTP 请求管道扩展类（中间件注册入口）
///
/// 作用：
/// 封装通用的中间件（Middleware）注册逻辑
///
/// 本质：
/// 对 ASP.NET Core 请求处理流水线（Pipeline）的统一配置
///
/// 对应执行阶段：
/// HTTP 请求进入后 → 按顺序执行这些中间件
/// </summary>
public static class AppBuilderExtensions
{
    /// <summary>
    /// 通用 HTTP 中间件注册方法
    ///
    /// 使用方式：
    /// app.UseHttpCommon();
    ///
    /// 作用：
    /// 将系统级别的中间件统一接入（健康检查、认证、异常等）
    /// </summary>
    public static IApplicationBuilder UseHttpCommon(this IApplicationBuilder app)
    {
        // =============================
        // 1. 获取健康检查配置
        // =============================
        // 从 DI 容器中获取 IOptions<T>
        // Options 模式（配置绑定）
        var serviceCheck = app.ApplicationServices
                              .GetRequiredService<IOptions<ServiceCheckConfiguration>>()
                              .Value;

        // =============================
        // 2. 注册健康检查中间件
        // =============================
        // Path 示例：/health
        // Consul 会定期访问这个地址判断服务是否存活
        app.UseHealthChecks(serviceCheck.Path);

        // =============================
        // 3. 启用认证中间件
        // =============================
        // Authentication（认证）：
        // 验证“你是谁”（JWT / Cookie / OAuth）
        app.UseAuthentication();

        // =============================
        // 4. 启用授权中间件
        // =============================
        // Authorization（授权）：
        // 判断“你有没有权限”
        app.UseAuthorization();

        // =============================
        // 5. 全局异常处理
        // =============================
        // UseExceptionHandler：
        // 捕获后续中间件或 Controller 抛出的异常
        // 
        // ⚠ 当前写法问题很大（后面详细说）
        app.UseExceptionHandler(_ => { });

        return app;
    }
}