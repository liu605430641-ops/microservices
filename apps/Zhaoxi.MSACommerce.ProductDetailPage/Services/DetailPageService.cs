using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.ProductDetailPage.Apis;
using Zhaoxi.MSACommerce.SharedKernel.Result;

namespace Zhaoxi.MSACommerce.ProductDetailPage.Services;

public class DetailPageService(
    IServiceClient<IProductServiceApi> productClient,
    IServiceClient<ICategoryServiceApi> categoryClient,
    IServiceClient<IBrandServiceApi> brandClient) : IDetailPageService
{
    public async Task<Result<Dictionary<string, object>>> GetSpuModel(long id)
    {
        var spuResponse = await productClient.ServiceApi.GetSpuAsync(id);
        if (!spuResponse.IsSuccessStatusCode || spuResponse.Content is null) 
            return Result.NotFound("商品不存在");

        var spu = spuResponse.Content;
        if (spu.Status == 0)
            return Result.NotFound("商品未上架");
        
        // 并发请求
        var brandTask = brandClient.ServiceApi.GetBrandAsync(spu.BrandId);
        var categoryTask = categoryClient.ServiceApi.GetParents(spu.CategoryId);
        var specTask = categoryClient.ServiceApi.GetSpecs(spu.CategoryId);
        var parameterTask = categoryClient.ServiceApi.GetParameters(spu.CategoryId);
        await Task.WhenAll(brandTask, categoryTask, specTask, parameterTask);

        // 获取结果
        var brandResponse = await brandTask;
        var categoryResponse = await categoryTask;
        var specResponse = await specTask;
        var parameterResponse = await parameterTask;
        
        var model = new Dictionary<string, object>
        {
            { "spu", spu },
            { "skus", spu.Skus },
            { "detail", spu.Detail },
            { "brand", brandResponse.Content },
            { "categories", categoryResponse.Content },
            { "specs", specResponse.Content },
            { "parametersGroup", parameterResponse.Content }
        };

        return Result.Success(model);
    }
}