using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prometheus;
using Zhaoxi.MSACommerce.Consul.ServiceRegistration;

namespace Zhaoxi.MSACommerce.HttpApi.Common;

public static class AppBuilderExtensions
{
    public static IApplicationBuilder UseHttpCommon(this IApplicationBuilder app)
    {
        //启动指标服务
        app.UseMetricServer();
        app.UseHttpMetrics();
        
        app.UseCors("AllowAny");

        var serviceCheck = app.ApplicationServices.GetRequiredService<IOptions<ServiceCheckConfiguration>>().Value;
        app.UseHealthChecks(serviceCheck.Path);

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseExceptionHandler(_ => { });
        
        return app;
    }
}
