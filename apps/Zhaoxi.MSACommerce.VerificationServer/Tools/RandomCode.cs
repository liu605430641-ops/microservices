namespace Zhaoxi.MSACommerce.VerificationServer.Tools;

public static class RandomCode
{
    public static string Generate(int length = 6)
    {
        var random = new Random();
        var code = "";
        for (var i = 0; i < length; i++)
        {
            code += random.Next(0, 10);  // 生成 0-9 的随机数字
        }
        return code;
    }
}