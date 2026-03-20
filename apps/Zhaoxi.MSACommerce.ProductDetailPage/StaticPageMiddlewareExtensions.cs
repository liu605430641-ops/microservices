namespace Zhaoxi.MSACommerce.ProductDetailPage;

/// <summary>
/// 扩展中间件
/// </summary>
public static class StaticPageMiddlewareExtensions
{
    public static IApplicationBuilder UseStaticPageMiddleware(this IApplicationBuilder app, string directoryPath)
    {
        return app.UseMiddleware<StaticPageMiddleware>(directoryPath);
    }
}