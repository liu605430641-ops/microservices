using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.ProductDetailPage.Apis;

namespace Zhaoxi.MSACommerce.ProductDetailPage;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureServiceClient(services, configuration);

        ConfigureCors(services);

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
