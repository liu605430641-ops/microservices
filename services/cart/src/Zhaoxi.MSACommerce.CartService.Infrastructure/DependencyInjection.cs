using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.CartService.Core.Data;
using Zhaoxi.MSACommerce.CartService.Infrastructure.Data;
using Zhaoxi.MSACommerce.Infrastructure.Common;
using Zhaoxi.MSACommerce.Infrastructure.Redis;

namespace Zhaoxi.MSACommerce.CartService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureCommon(configuration);
        services.AddInfrastructureRedis(configuration);
        
        services.AddScoped<ICartRepository, RedisCartRepository>();
        return services;
    }
}