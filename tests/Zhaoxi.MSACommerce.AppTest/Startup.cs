using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Zhaoxi.MSACommerce.AppTest;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        //代码注释 masstransit 是一个开源的分布式消息发送，提供了一个简单的API来构建基于消息的应用程序。它支持多种消息传递协议和消息代理，如RabbitMQ、Azure Service Bus等。通过使用MassTransit，开发人员可以轻松地实现消息发布/订阅、请求/响应等模式，从而构建可伸缩、可靠的分布式系统。
        
        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://rabbitmq:123123@34.96.203.128:5672");

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}