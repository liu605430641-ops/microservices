using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text;
using CaptchaGen.NetCore;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases.Commands;

public record CreateSecKillLinkCommand(long Id) : ICommand<Result<string>>;

public class CreateSecKillLinkCommandHandler(IConnectionMultiplexer redis, IUser user) : ICommandHandler<CreateSecKillLinkCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateSecKillLinkCommand request, CancellationToken cancellationToken)
    {
        var inputBytes = Encoding.UTF8.GetBytes($"{request.Id}{user.Id}");
        var hashBytes = MD5.HashData(inputBytes);
        // 将字节数组转换为十六进制字符串
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2")); 
        }
        var link = sb.ToString();
        
        var db = redis.GetDatabase();
        var key = $"{RedisKeyConstants.SecKillLinkPrefix}{user.Id}";
        await db.HashSetAsync(key,request.Id, link);
        db.KeyExpire(key, TimeSpan.FromSeconds(60));
        return Result.Success(link);
    }
}