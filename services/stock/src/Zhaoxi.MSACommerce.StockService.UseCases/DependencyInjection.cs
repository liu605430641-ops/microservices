using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.StockService.UseCases.CapSubscribes;
using Zhaoxi.MSACommerce.UseCases.Common;

namespace Zhaoxi.MSACommerce.StockService.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        services.AddUseCaseCommon(Assembly.GetExecutingAssembly());

        services.AddTransient<IOrderSubscriber, OrderSubscriber>();
        
        return services;
    }
}
