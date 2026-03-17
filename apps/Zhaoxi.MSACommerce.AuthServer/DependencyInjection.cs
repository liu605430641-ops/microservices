using Refit;
using Zhaoxi.MSACommerce.AuthServer.Services;

namespace Zhaoxi.MSACommerce.AuthServer;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpApi(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureUserService(services, configuration);

        ConfigureIdentity(services, configuration);

        return services;
    }

    private static void ConfigureUserService(IServiceCollection services, IConfiguration configuration)
    {
        var userServiceUrl = configuration["ServiceUrls:UserService"];
        if (userServiceUrl is null) throw new NullReferenceException(nameof(userServiceUrl));

        services.AddRefitClient<IUserService>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(userServiceUrl);
            });
    }

    private static void ConfigureIdentity(IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<IIdentityService, IdentityService>();

        // 从配置文件中读取JwtSettings，并注入到容器中
        var configurationSection = configuration.GetSection(nameof(JwtSettings));
        var jwtSettings = configurationSection.Get<JwtSettings>();
        if (jwtSettings is null) throw new NullReferenceException(nameof(jwtSettings));
        services.Configure<JwtSettings>(configurationSection);
    }
}
