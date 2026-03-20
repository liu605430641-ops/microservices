using Refit;
using Zhaoxi.MSACommerce.ElasticSyncWorker.Models;

namespace Zhaoxi.MSACommerce.ElasticSyncWorker.Apis;

public interface ICategoryServiceApi
{
    [Get("/api/category/parents")]
    Task<ApiResponse<List<CategoryDto>>> GetParents(long id);

    [Get("/api/category/specs")]
    Task<ApiResponse<List<SpecKeyDto>>> GetSpecs(long id);

    [Get("/api/category/parameters")]
    Task<ApiResponse<List<ParameterGroupDto>>> GetParameters(long id);
}