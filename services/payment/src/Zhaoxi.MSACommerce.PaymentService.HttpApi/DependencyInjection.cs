using Zhaoxi.MSACommerce.HttpApi.Common;

namespace Zhaoxi.MSACommerce.PaymentService.HttpApi;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpApi(this IServiceCollection services)
    {
        services.AddHttpApiCommon();
        
        return services;
    }
}
