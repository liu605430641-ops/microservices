using Zhaoxi.MSACommerce.BrandService.UseCases.Commands;
using Zhaoxi.MSACommerce.CategoryService.Core.Entities;

namespace Zhaoxi.MSACommerce.BrandService.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateBrandCommand, Brand>();
        CreateMap<Brand, BrandDto>();
    }
}
