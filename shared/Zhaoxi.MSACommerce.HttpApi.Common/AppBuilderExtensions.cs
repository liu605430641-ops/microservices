using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Zhaoxi.MSACommerce.Consul.ServiceRegistration;

namespace Zhaoxi.MSACommerce.HttpApi.Common;

public static class AppBuilderExtensions
{
    public static IApplicationBuilder UseHttpCommon(this IApplicationBuilder app)
    {
        app.UseCors("AllowAny");

        var serviceCheck = app.ApplicationServices.GetRequiredService<IOptions<ServiceCheckConfiguration>>().Value;
        app.UseHealthChecks(serviceCheck.Path);

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseExceptionHandler(_ => { });
        
        return app;
    }
}
