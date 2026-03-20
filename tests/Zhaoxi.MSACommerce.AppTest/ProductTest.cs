using MassTransit;
using Zhaoxi.MSACommerce.SharedEvent.Products;

namespace Zhaoxi.MSACommerce.AppTest;

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