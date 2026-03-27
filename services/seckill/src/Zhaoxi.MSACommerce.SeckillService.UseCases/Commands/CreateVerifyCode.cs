using System.Drawing.Imaging;
using CaptchaGen.NetCore;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases.Commands;

public record CreateVerifyCodeCommand(int Count) : ICommand<Result<MemoryStream>>;

public class CreateVerifyCodeCommandHandler(IConnectionMultiplexer redis, IUser user) : ICommandHandler<CreateVerifyCodeCommand, Result<MemoryStream>>
{
    public async Task<Result<MemoryStream>> Handle(CreateVerifyCodeCommand request, CancellationToken cancellationToken)
    {
        var db = redis.GetDatabase();
        var code = ImageFactory.CreateCode(request.Count);
        await db.StringSetAsync($"{RedisKeyConstants.SecKillVerifyCodePrefix}{user.Id}", code, TimeSpan.FromSeconds(60));
        var image = ImageFactory.BuildImage(code, 32, 80, 16, 10, ImageFormat.Jpeg);

        return image is null ? Result.Failure("验证码生成失败") : Result.Success(image);
    }
}