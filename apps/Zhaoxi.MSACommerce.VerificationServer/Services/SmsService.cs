using StackExchange.Redis;
using Zhaoxi.MSACommerce.SharedKernel.Result;
using Zhaoxi.MSACommerce.VerificationServer.Tools;

namespace Zhaoxi.MSACommerce.VerificationServer.Services;

public class SmsService(IConnectionMultiplexer redis) : ISmsService
{
    private readonly TimeSpan _expiry = TimeSpan.FromMinutes(5);
    private const string SmsKeyPrefix = "sms";
    
    public async Task<Result> SendCodeAsync(string phoneNumber)
    {
        var db = redis.GetDatabase();
        var limitPrefix = $"{SmsKeyPrefix}:limit";
            
        
        
        // 验证1分钟内是否已发送
        var limitTimeKey = $"{limitPrefix}:time:{phoneNumber}";
        var limitTime = await db.StringGetAsync(limitTimeKey);
        if (!limitTime.IsNull) return Result.Failure("请求过于频繁，请稍后再试");
        await db.StringSetAsync(limitTimeKey, 0, TimeSpan.FromMinutes(1));
        
        
        
        // 验证发送频次
        const int window = 60 * 60 * 24; // 时间窗口为 24 小时秒
        const int limit = 10; // 24小时内最大请求次数
        // 每个时间窗口生成一个唯一键
        var limitWindowKey = $"{limitPrefix}:window:{phoneNumber}:{DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond / window}"; 
        // 增加请求计数
        var currentCount = await db.StringIncrementAsync(limitWindowKey);
        // 第一次请求，设置过期时间
        if (currentCount == 1) await db.KeyExpireAsync(limitWindowKey, TimeSpan.FromSeconds(window));
        // 如果请求次数超过限制，则返回错误
        if (currentCount > limit) return Result.Failure("请求过于频繁，请稍后再试");
        
        
        
        // 生成随机验证码
        var code = RandomCode.Generate();
        // 发送验证码（假装发送，并保存到 Redis）
        var sendKey = $"{SmsKeyPrefix}:{phoneNumber}";
        await db.StringSetAsync(sendKey, code, _expiry);
        Console.WriteLine($"发送验证码 [{code}] 到 [{phoneNumber}] (有效期 [{_expiry.TotalMinutes}] 分钟)");

        return Result.Success();
    }

    public async Task<Result> VerifyCodeAsync(string phoneNumber, string inputCode)
    {
        // 从 Redis 获取验证码
        var db = redis.GetDatabase();
        var key = $"{SmsKeyPrefix}:{phoneNumber}";
        var storedCode = await db.StringGetAsync(key);
        
        // 验证码是否过期
        if (storedCode.IsNull) return Result.Failure("验证码已过期或不存在");

        // 验证用户输入的验证码是否匹配
        if (storedCode != inputCode) return Result.Failure("验证码已过期或不存在");
        
        // 验证成功后删除验证码
        db.KeyDelete(key);  
        
        return Result.Success();
    }
}