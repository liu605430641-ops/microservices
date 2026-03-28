using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.PaymentService.UseCases.Apis;
using Zhaoxi.MSACommerce.PaymentService.UseCases.CapSubscribes;
using Zhaoxi.MSACommerce.UseCases.Common;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        services.AddUseCaseCommon(Assembly.GetExecutingAssembly());
        
        services.AddTransient<IOrderSubscriber, OrderSubscriber>();
        
        ConfigureServiceClient(services);
        
        return services;
    }
    
    private static void ConfigureServiceClient(IServiceCollection services)
    {
        services.AddServiceClient<IOrderServiceApi>(option =>
        {
            option.ServiceName = "Zhaoxi.MSACommerce.OrderService.HttpApi";
            option.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
        }, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(2);
        });
        
        services.AddServiceClient<ISeckillServiceApi>(option =>
        {
            option.ServiceName = "Zhaoxi.MSACommerce.SeckillService.HttpApi";
            option.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
        }, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(2);
        });
        
    }
}
