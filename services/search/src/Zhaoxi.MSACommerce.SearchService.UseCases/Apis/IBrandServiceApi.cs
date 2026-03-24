using Refit;

namespace Zhaoxi.MSACommerce.SearchService.UseCases.Apis;

public interface IBrandServiceApi
{
    [Get("/api/brand/list")]
    Task<ApiResponse<List<BrandDto>>> GetBrandsAsync([Body]long[] ids);
}