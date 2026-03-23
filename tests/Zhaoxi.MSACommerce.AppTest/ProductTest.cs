using MassTransit;
using Zhaoxi.MSACommerce.SharedEvent.Products;

namespace Zhaoxi.MSACommerce.AppTest;

/// <summary>
/// 生产者
/// 消费者是:Zhaoxi.MSACommerce.StaticPageWorker.Consumers.ProductUpdatedConsumer
/// </summary>
/// <param name="publishEndpoint"></param>
public class ProductTest(IPublishEndpoint publishEndpoint)
{
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public async void UpdateProductTest(long id)
    {
        await publishEndpoint.Publish(new ProductUpdatedEvent(id));
    }
    
    [Fact]
    public async void ProductFullSyncTest()
    {
        await publishEndpoint.Publish(new ProductFullSyncEvent());
    }
}