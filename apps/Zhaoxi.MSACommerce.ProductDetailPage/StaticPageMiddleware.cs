namespace Zhaoxi.MSACommerce.ProductDetailPage;

/// <summary>
/// 支持在返回HTML时，将返回的Stream保存到指定目录
/// </summary>
public class StaticPageMiddleware(
    RequestDelegate next,
    string directoryPath)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/item"))
        {
            var originalBody = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;
            await next(context);

            // 在请求处理完成后，保存缓存文件
            if (memoryStream.Length > 0)
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var htmlContent = await new StreamReader(memoryStream).ReadToEndAsync();

                // 将 HTML 内容写入缓存文件
                SaveHtml(context.Request.Path, htmlContent);

                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBody);
            }
        }
        else
        {
            // 对于不需要缓存的请求，继续处理
            await next(context);
        }
    }

    private void SaveHtml(string url, string html)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(html))
                return;
            if (!url.EndsWith(".html"))
                return;

            if (Directory.Exists(directoryPath) == false)
                Directory.CreateDirectory(directoryPath);

            var totalPath = Path.Combine(directoryPath, url.Split("/").Last());
            // 直接覆盖---要不要检测一下呢？不用，因为一旦访问动态页，其实就是要去覆盖
            File.WriteAllText(totalPath, html);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}