using Zhaoxi.MSACommerce.SharedKernel.Result;

namespace Zhaoxi.MSACommerce.AuthServer.Services;

public interface IIdentityService
{
    Task<Result<string>> GetAccessTokenAsync(string username, string password);
}
