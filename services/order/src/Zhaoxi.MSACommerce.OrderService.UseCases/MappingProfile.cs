using Zhaoxi.MSACommerce.OrderService.Core.Entities;

namespace Zhaoxi.MSACommerce.OrderService.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order,OrderDto>();
    }
}