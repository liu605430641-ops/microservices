using Refit;
using Zhaoxi.MSACommerce.ProductDetailPage.Models;

namespace Zhaoxi.MSACommerce.ProductDetailPage.Apis;

public interface IBrandServiceApi
{
    [Get("/api/brand")]
    Task<ApiResponse<BrandDto>> GetBrandAsync(long id);
}