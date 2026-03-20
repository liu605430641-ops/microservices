using Refit;
using Zhaoxi.MSACommerce.ElasticSyncWorker.Models;

namespace Zhaoxi.MSACommerce.ElasticSyncWorker.Apis;

public interface IBrandServiceApi
{
    [Get("/api/brand")]
    Task<ApiResponse<BrandDto>> GetBrandAsync(long id);
}