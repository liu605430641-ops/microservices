using Refit;

namespace Zhaoxi.MSACommerce.AuthServer.Services;

public record UserDto(long Id, string Username, string? Phone);

public interface IUserService
{
    [Get("/api/user")]
    Task<ApiResponse<UserDto>> GetUserAsync(string username, string password);
}
