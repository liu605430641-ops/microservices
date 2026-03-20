using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.ProductService.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Spu, SpuDto>();
        CreateMap<SpuDetail, SpuDetailDto>();
        CreateMap<Sku, SkuDto>();
    }
}
