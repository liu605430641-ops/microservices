using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.SearchService.UseCases.Apis;
using Zhaoxi.MSACommerce.UseCases.Common;

namespace Zhaoxi.MSACommerce.SearchService.UseCases;

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
        services.AddServiceClient<ICategoryServiceApi>(option =>
        {
            option.ServiceName = "Zhaoxi.MSACommerce.CategoryService.HttpApi";
            option.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
        }, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(2);
        });
        
        services.AddServiceClient<IBrandServiceApi>(option =>
        {
            option.ServiceName = "Zhaoxi.MSACommerce.BrandService.HttpApi";
            option.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
        }, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(2);
        });
    }

}
