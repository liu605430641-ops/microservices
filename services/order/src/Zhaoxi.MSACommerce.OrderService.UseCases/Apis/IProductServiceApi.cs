using Refit;

namespace Zhaoxi.MSACommerce.OrderService.UseCases.Apis;

public interface IProductServiceApi
{
    [Get("/api/product/sku/list")]
    Task<ApiResponse<List<SkuDto>>> GetSkuListAsync([Body]long[] ids);
}