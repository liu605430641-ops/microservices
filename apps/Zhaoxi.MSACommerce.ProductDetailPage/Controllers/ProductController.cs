using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.ProductDetailPage.Services;
using Zhaoxi.MSACommerce.SharedKernel.Result;

namespace Zhaoxi.MSACommerce.ProductDetailPage.Controllers;

public class ProductController(IDetailPageService pageDetailService) : Controller
{
    [Route("/item/{id}.html")]
    public async Task<IActionResult> Index(long id)
    {
        var result = await pageDetailService.GetSpuModel(id);
        
        if (!result.IsSuccess) return NotFound();
        
        return View(result.Value);
    }
}