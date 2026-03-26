namespace Zhaoxi.MSACommerce.UserService.UseCases.Queries;

public record GetUserByUsernameQuery(string Username) : IQuery<Result>;

public class GetUserByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
    public GetUserByUsernameQueryValidator()
    {
        RuleFor(query => query.Username)
            .NotEmpty();
    }
}

public class GetUserByUsernameQueryHandler(UserDbContext dbContext, IMapper mapper) : IQueryHandler<GetUserByUsernameQuery, Result>
{
    public async Task<Result> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.TbUsers.AsNoTracking()
            .Where(tbUser => tbUser.Username == request.Username)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return user == null ? Result.NotFound() : Result.Success();
    }
}