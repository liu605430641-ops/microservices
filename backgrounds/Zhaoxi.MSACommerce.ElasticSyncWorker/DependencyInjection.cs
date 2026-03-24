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
    
    /// <summary>
    /// 注册依赖的服务客户端
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
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

    /// <summary>
    /// 自注册消费者和MassTransit
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
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

    /// <summary>
    /// 链接es，并注册ElasticsearchClient
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <exception cref="ArgumentNullException"></exception>
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
