using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.UseCases.Common;

namespace Zhaoxi.MSACommerce.CategoryService.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        services.AddUseCaseCommon(Assembly.GetExecutingAssembly());

        return services;
    }
}
