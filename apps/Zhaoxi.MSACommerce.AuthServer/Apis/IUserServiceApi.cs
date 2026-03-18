using Refit;

namespace Zhaoxi.MSACommerce.AuthServer.Apis;

public record UserDto(long Id, string Username, string? Phone);

public interface IUserServiceApi
{
    [Get("/api/user")]
    Task<ApiResponse<UserDto>> GetUserAsync(string username, string password);
}