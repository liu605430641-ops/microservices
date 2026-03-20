using Microsoft.AspNetCore.Mvc;
using Zhaoxi.MSACommerce.CategoryService.UseCases.Queries;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;

namespace Zhaoxi.MSACommerce.CategoryService.HttpApi.Controllers;

[Route("api/category")]
[ApiController]
public class CategoryController() : ApiControllerBase
{
    [HttpGet("parents")]
    public async Task<IActionResult> GetParents(long id)
    {
        var result = await Sender.Send(new GetCategoryAndParentsQuery(id));
        return ReturnResult(result);
    }
    
    [HttpGet("children")]
    public async Task<IActionResult> GetChildren(long id)
    {
        var result = await Sender.Send(new GetCategoryAndChildrenQuery(id));
        return ReturnResult(result);
    }
    
    [HttpGet("specs")]
    public async Task<IActionResult> GetSpecs(long id)
    {
        var result = await Sender.Send(new GetSpecsByCategoryQuery(id));
        return ReturnResult(result);
    }
    
    [HttpGet("parameters")]
    public async Task<IActionResult> GetParameters(long id)
    {
        var result = await Sender.Send(new GetParemetersByCategoryQuery(id));
        return ReturnResult(result);
    }
}