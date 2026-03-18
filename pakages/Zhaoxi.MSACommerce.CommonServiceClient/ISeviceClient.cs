namespace Zhaoxi.MSACommerce.LoadBalancer;

public interface IServiceClient<TServiceApi> where TServiceApi : class
{
    string ServiceName { get; set; }

    TServiceApi ServiceApi { get; set; }
}