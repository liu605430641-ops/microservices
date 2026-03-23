using MassTransit;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.SharedEvent.Products;
using Zhaoxi.MSACommerce.StaticPageWorker.Apis;

namespace Zhaoxi.MSACommerce.StaticPageWorker.Consumers;

/// <summary>
/// 消费者
/// 生产者是:Zhaoxi.MSACommerce.AppTest.ProductTest
/// </summary>
/// <param name="client"></param>
public class ProductUpdatedConsumer(IServiceClient<IProductDetailPage> client) : IConsumer<ProductUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
    {
        Console.WriteLine("ProductUpdatedEvent: {0}", context.Message.SpuId);
        var result = await client.ServiceApi.DeletePageAsync(context.Message.SpuId);
        Console.WriteLine("DeletePage: {0}", result.IsSuccessStatusCode);
    }
}