using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.HttpApi.Common.Services;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.HttpApi.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpApiCommon(this IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddExceptionHandler<UseCaseExceptionHandler>();

        services.AddProblemDetails();

        ConfigureCors(services);

        return services;
    }

    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}
