using Refit;
using Zhaoxi.MSACommerce.ElasticSyncWorker.Models;
using Zhaoxi.MSACommerce.SharedKernel.Paging;

namespace Zhaoxi.MSACommerce.ElasticSyncWorker.Apis;

public interface IProductServiceApi
{
    [Get("/api/product/spu")]
    Task<ApiResponse<SpuDto>> GetSpuAsync(long id);
    
    [Get("/api/product/spu/list")]
    Task<ApiResponse<List<SpuDto>>> GetSpuListAsync(int pageNumber, int pageSize);
}