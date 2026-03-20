using Zhaoxi.MSACommerce.SharedKernel.Result;

namespace Zhaoxi.MSACommerce.ProductDetailPage.Services;

public class StaticPageService(IConfiguration configuration) : IStaticPageService
{
    private string StaticPagePath { get; } = configuration["StaticPagePath"] ?? "wwwroot";
    
    public Result DeletePage(long id)
    {
        var pagePath = Path.Combine(StaticPagePath, id + ".html");
        if (!File.Exists(pagePath)) return Result.Success();
        try
        {
            File.Delete(pagePath);
            return Result.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Result.Failure("删除失败");
        }
        
    }
}