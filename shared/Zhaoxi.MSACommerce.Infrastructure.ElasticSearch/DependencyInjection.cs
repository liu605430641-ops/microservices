using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zhaoxi.MSACommerce.Infrastructure.ElasticSearch;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEs(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureElasticSearch(services, configuration);

        return services;
    }
    

    private static void ConfigureElasticSearch(IServiceCollection services, IConfiguration configuration)
    {
        var esConn = configuration.GetConnectionString("ElasticSearchConnection");
        if (string.IsNullOrEmpty(esConn)) throw new ArgumentNullException("ElasticSearchConnection");

        var settings = new ElasticsearchClientSettings(new Uri(esConn)) 
           .Authentication(new BasicAuthentication("elastic", "123123"));;

        var client = new ElasticsearchClient(settings);
       
        services.AddSingleton(client);
    }
}
