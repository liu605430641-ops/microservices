using Zhaoxi.MSACommerce.SearchService.Core.Entities;
using Zhaoxi.MSACommerce.SharedKernel.Paging;

namespace Zhaoxi.MSACommerce.SearchService.UseCases;

public record SearchProductDto
{
    public List<BrandDto> Brands { get; set; }
    public List<CategoryDto> Categories { get; set; }
    public PagedList<Product> Products { get; set; }
    public PagedMetaData Page { get; set; }
}