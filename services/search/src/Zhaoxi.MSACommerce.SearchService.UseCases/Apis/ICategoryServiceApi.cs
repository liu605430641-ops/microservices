using Refit;

namespace Zhaoxi.MSACommerce.SearchService.UseCases.Apis;

public interface ICategoryServiceApi
{
    [Get("/api/category/list")]
    Task<ApiResponse<List<CategoryDto>>> GetListAsync([Body]long[] ids);
}