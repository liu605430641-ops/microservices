using Zhaoxi.MSACommerce.UserService.Infrastructure.Tools;

namespace Zhaoxi.MSACommerce.UserService.UseCases.Queries;

public record UserDto(long Id, string Username, string? Phone);

public record GetUserQuery(string Username, string Password) : IQuery<Result<UserDto>>;

public class GetQuestionQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetQuestionQueryValidator()
    {
        RuleFor(query => query.Username)
            .NotEmpty();
        RuleFor(query => query.Password)
            .NotEmpty();
    }
}

public class GetUserQueryHandler(UserDbContext dbContext, IMapper mapper) : IQueryHandler<GetUserQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.TbUsers.AsNoTracking()
            .Where(tbUser => tbUser.Username == request.Username)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (user == null) return Result.NotFound();

        if (Md5Helper.MD5EncodingWithSalt(request.Password, user.Salt) != user.Password)
        {
            return Result.Failure("密码不正确");
        }

        var userDto = mapper.Map<UserDto>(user);

        return Result.Success(userDto);
    }
}
