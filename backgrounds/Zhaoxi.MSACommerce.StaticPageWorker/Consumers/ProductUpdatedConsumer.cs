using MassTransit;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.SharedEvent.Products;
using Zhaoxi.MSACommerce.StaticPageWorker.Apis;

namespace Zhaoxi.MSACommerce.StaticPageWorker.Consumers;

public class ProductUpdatedConsumer(IServiceClient<IProductDetailPage> client) : IConsumer<ProductUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
    {
        Console.WriteLine("ProductUpdatedEvent: {0}", context.Message.SpuId);
        var result = await client.ServiceApi.DeletePageAsync(context.Message.SpuId);
        Console.WriteLine("DeletePage: {0}", result.IsSuccessStatusCode);
    }
}