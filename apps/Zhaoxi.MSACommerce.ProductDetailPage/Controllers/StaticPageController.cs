using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.ProductDetailPage.Services;

namespace Zhaoxi.MSACommerce.ProductDetailPage.Controllers;
 
[ApiController]
public class StaticPageController(IStaticPageService staticPageService) : ControllerBase
{
    [HttpDelete("/item/{id:long}.html")]
    public IActionResult Delete(long id)
    {
        var result = staticPageService.DeletePage(id);
        return result.IsSuccess ? Ok() : BadRequest(result.Errors);
    }
}