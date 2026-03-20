using Refit;

namespace Zhaoxi.MSACommerce.StaticPageWorker.Apis;

public interface IProductDetailPage
{
     [Delete("/item/{id}.html")]
     Task<IApiResponse> DeletePageAsync(long id);
}