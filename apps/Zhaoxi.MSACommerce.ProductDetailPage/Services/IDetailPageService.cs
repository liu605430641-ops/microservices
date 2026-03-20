using Zhaoxi.MSACommerce.SharedKernel.Result;

namespace Zhaoxi.MSACommerce.ProductDetailPage.Services;

public interface IDetailPageService
{
    Task<Result<Dictionary<string, object>>> GetSpuModel(long id);
}