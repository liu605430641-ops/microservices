using Refit;

namespace Zhaoxi.MSACommerce.PaymentService.UseCases.Apis;
    
public interface ISeckillServiceApi
{
    [Get("/api/seckill/order/{userId}")]
    Task<IApiResponse<SecKillOrderDto>> GetOrderAsync(long userId);
}