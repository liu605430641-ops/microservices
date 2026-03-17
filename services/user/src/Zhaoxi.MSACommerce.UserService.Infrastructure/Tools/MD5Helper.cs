using System.Security.Cryptography;
using System.Text;

namespace Zhaoxi.MSACommerce.UserService.Infrastructure.Tools;

/// <summary>
///     加密用的
/// </summary>
public static class Md5Helper
{
    /// <summary>
    ///     MD5 加密字符串
    /// </summary>
    /// <param name="content">源字符串</param>
    /// <returns>加密后字符串</returns>
    public static string MD5EncodingOnly(string content)
    {
        // 创建MD5类的默认实例：MD5CryptoServiceProvider
        var md5 = MD5.Create();
        var bs = Encoding.UTF8.GetBytes(content);
        var hs = md5.ComputeHash(bs);
        var stb = new StringBuilder();
        foreach (var b in hs)
            // 以十六进制格式格式化
            stb.Append(b.ToString("x2"));
        return stb.ToString();
    }

    /// <summary>
    ///     MD5盐值加密
    /// </summary>
    /// <param name="content">源字符串</param>
    /// <param name="salt">盐值</param>
    /// <returns>加密后字符串</returns>
    public static string MD5EncodingWithSalt(string content, string salt)
    {
        return string.IsNullOrEmpty(salt) ? content : MD5EncodingOnly(content + "{" + salt + "}");
    }
}
