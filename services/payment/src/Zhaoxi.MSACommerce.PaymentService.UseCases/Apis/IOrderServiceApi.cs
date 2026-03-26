using Refit;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.Apis;
    
public interface IOrderServiceApi
{
    [Get("/api/order")]
    Task<IApiResponse<OrderDto>> GetOrderAsync(long id);
}