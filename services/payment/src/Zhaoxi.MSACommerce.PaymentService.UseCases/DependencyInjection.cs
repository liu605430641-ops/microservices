using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.PaymentService.UseCases.Apis;
using Zhaoxi.MSACommerce.UseCases.Common;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        services.AddUseCaseCommon(Assembly.GetExecutingAssembly());

        ConfigureServiceClient(services);

        return services;
    }

    private static void ConfigureServiceClient(IServiceCollection services)
    {
        services.AddServiceClient<IOrderServiceApi>(option =>
                                                    {
                                                        option.ServiceName           = "Zhaoxi.MSACommerce.OrderService.HttpApi";
                                                        option.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
                                                    },client => { client.Timeout = TimeSpan.FromSeconds(2); }
                                                   );
    }
}