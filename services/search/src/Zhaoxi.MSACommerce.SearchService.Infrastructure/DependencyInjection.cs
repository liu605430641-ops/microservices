using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhaoxi.MSACommerce.Infrastructure.Common;
using Zhaoxi.MSACommerce.Infrastructure.ElasticSearch;

namespace Zhaoxi.MSACommerce.SearchService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureCommon(configuration);
        services.AddInfrastructureEs(configuration);
        return services;
    }
}