using Zhaoxi.MSACommerce.UserService.Core;
using Zhaoxi.MSACommerce.UserService.Core.Entities;
using Zhaoxi.MSACommerce.UserService.Infrastructure.Tools;

namespace Zhaoxi.MSACommerce.UserService.UseCases.Commands;

public record CreateUserCommand(string Username, string Password, string? Phone) : ICommand<Result>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.Username)
            .NotEmpty()
            .MaximumLength(DataSchemaConstants.DefaultUsernameMaxLength);

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(DataSchemaConstants.DefaultPasswordMaxLength);

        RuleFor(command => command.Phone)
            .Length(DataSchemaConstants.DefaultPhoneLength);
    }
}

public class CreateUserCommandHanlder(UserDbContext dbContext, IMapper mapper) : ICommandHandler<CreateUserCommand, Result>
{
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await dbContext.TbUsers
                .AnyAsync(user => user.Username == request.Username, cancellationToken: cancellationToken);

        if (userExists)
        {
            return Result.Failure("用户名已存在");
        }

        var user = mapper.Map<TbUser>(request);

        user.Salt = Md5Helper.MD5EncodingOnly(user.Username);
        user.Password = Md5Helper.MD5EncodingWithSalt(user.Password, user.Salt);

        dbContext.TbUsers.Add(user);
        var count = await dbContext.SaveChangesAsync(cancellationToken);

        return count != 1 ? Result.Failure("用户注册失败") : Result.Success();
    }
}
