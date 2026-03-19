using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using Zhaoxi.MSACommerce.CategoryService.UseCases.Commands;

namespace Zhaoxi.MSACommerce.CategoryService.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCategoryCommand, Category>();
    }
}
