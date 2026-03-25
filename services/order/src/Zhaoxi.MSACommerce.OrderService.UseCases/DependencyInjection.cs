using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.OrderService.UseCases.Apis;
using Zhaoxi.MSACommerce.UseCases.Common;

namespace Zhaoxi.MSACommerce.OrderService.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        services.AddUseCaseCommon(Assembly.GetExecutingAssembly());

        return services;
    }
    
    private static void ConfigureServiceClient(IServiceCollection services)
    {
        services.AddServiceClient<IStockServiceApi>(option =>
        {
            option.ServiceName = "Zhaoxi.MSACommerce.CategoryService.HttpApi";
            option.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
        }, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(2);
        });
    }
}
