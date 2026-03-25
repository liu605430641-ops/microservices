using Refit;

namespace Zhaoxi.MSACommerce.OrderService.UseCases.Apis;
    
public interface IStockServiceApi
{
    [Post("/api/stock/resv")]
    Task<IApiResponse> CreateStockResvAsync(long skuId, long orderId, int quantity);
}