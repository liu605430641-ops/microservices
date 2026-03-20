using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using MassTransit;
using Zhaoxi.MSACommerce.ElasticSyncWorker.Apis;
using Zhaoxi.MSACommerce.ElasticSyncWorker.Consumers;
using Zhaoxi.MSACommerce.LoadBalancer;

namespace Zhaoxi.MSACommerce.ElasticSyncWorker;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureServiceClient(services, configuration);

        ConfigureMassTransit(services, configuration);

        ConfigureElasticSearch(services, configuration);

        return services;
    }
    
    private static void ConfigureServiceClient(IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceClient<IProductServiceApi>(option =>
        {
            option.ServiceName = "Zhaoxi.MSACommerce.ProductService.HttpApi";
            option.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
        }, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(2);
        });
        
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

    private static void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<ProductFullSyncConsumer>();
            configurator.UsingRabbitMq((context, cfg) =>
            {

                cfg.Host(configuration.GetConnectionString("RabbitMqConnection"));
                cfg.ConfigureEndpoints(context);
            });
        });
    }

    private static void ConfigureElasticSearch(IServiceCollection services, IConfiguration configuration)
    {
        var esConn = configuration.GetConnectionString("ElasticSearchConnection");
        if (string.IsNullOrEmpty(esConn)) throw new ArgumentNullException("ElasticSearchConnection");

        var settings = new ElasticsearchClientSettings(new Uri(esConn));

        var client = new ElasticsearchClient(settings);

        services.AddSingleton(client);
    }
}
