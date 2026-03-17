using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.HttpApi.Common.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

    public int? Id
    {
        get
        {
            var id = _user?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id is null) return null;

            return Convert.ToInt32(id);
        }
    }

    public string? Username => _user?.FindFirstValue(ClaimTypes.Name);
}
