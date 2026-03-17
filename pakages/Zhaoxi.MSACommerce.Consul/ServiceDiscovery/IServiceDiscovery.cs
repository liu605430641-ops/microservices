namespace Zhaoxi.MSACommerce.Consul.ServiceDiscovery;

public interface IServiceDiscovery
{
    Task<List<string>> GetServicesAsync(string serviceName);


}
