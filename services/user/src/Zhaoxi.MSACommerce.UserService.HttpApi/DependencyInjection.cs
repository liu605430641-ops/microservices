using Zhaoxi.MSACommerce.HttpApi.Common;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.UserService.HttpApi.Apis;

namespace Zhaoxi.MSACommerce.UserService.HttpApi;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpApi(this IServiceCollection services)
    {
        services.AddHttpApiCommon();
        ConfigureVerificationServer(services);
        return services;
    }

    private static void ConfigureVerificationServer(IServiceCollection services)
    {

        services.AddServiceClient<IVerificationApi>(options =>
                                                    {
                                                        
                                                        
                                                        options.ServiceName           = "Zhaoxi.MSACommerce.VerificationServer";
                                                        options.LoadBalancingStrategy = LoadBalancingStrategy.RoundRobin;
                                                    },client =>
                                                      {

                                                          client.Timeout = TimeSpan.FromDays(3);
                                                      }
                                                   );
    }
   

}
