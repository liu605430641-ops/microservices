using Zhaoxi.MSACommerce.CategoryService.Core.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Zhaoxi.MSACommerce.BrandService.UseCases.Commands;

public record CreateBrandCommand(string Name, string Image, string Letter) : ICommand<Result>;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(query => query.Name)
            .NotEmpty();
        RuleFor(query => query.Letter)
            .NotEmpty();
    }
}

public class CreateBrandCommandHandler(BrandDbContext dbContext, IFusionCache cache, IMapper mapper) : ICommandHandler<CreateBrandCommand, Result>
{
    public async Task<Result> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var category = mapper.Map<Brand>(request);
        dbContext.Add(category);
        var count = await dbContext.SaveChangesAsync(cancellationToken);
        if (count <= 0) return Result.Failure();
        
        // 删除缓存
        await cache.RemoveAsync(nameof(Brand), token: cancellationToken);
        return Result.Success();
    }
}