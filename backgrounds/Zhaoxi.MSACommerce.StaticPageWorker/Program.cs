using Consul.AspNetCore;
using MassTransit;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.StaticPageWorker.Apis;
using Zhaoxi.MSACommerce.StaticPageWorker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddConsul();

builder.Services.AddServiceClient<IProductDetailPage>(option =>
{
    option.ServiceName = "Zhaoxi.MSACommerce.ProductDetailPage";
    option.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
}, client =>
{
    client.Timeout = TimeSpan.FromSeconds(2);
});

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<ProductUpdatedConsumer>();
    configurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMqConnection"));
        cfg.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(10)));
        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();