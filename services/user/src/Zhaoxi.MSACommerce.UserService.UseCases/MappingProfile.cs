using Zhaoxi.MSACommerce.UserService.Core.Entities;
using Zhaoxi.MSACommerce.UserService.UseCases.Commands;
using Zhaoxi.MSACommerce.UserService.UseCases.Queries;

namespace Zhaoxi.MSACommerce.UserService.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserCommand, TbUser>();

        CreateMap<TbUser, UserDto>();
    }
}
