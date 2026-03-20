using Refit;
using Zhaoxi.MSACommerce.ProductDetailPage.Models;

namespace Zhaoxi.MSACommerce.ProductDetailPage.Apis;

public interface IProductServiceApi
{
    [Get("/api/product/spu")]
    Task<ApiResponse<SpuDto>> GetSpuAsync(long id);
}