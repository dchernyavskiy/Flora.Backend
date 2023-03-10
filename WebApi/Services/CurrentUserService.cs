using System.Security.Claims;
using Flora.Application.Common.Interfaces;

namespace Flora.WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                                     string.Empty);

    public string Role => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    public string FirstName => _httpContextAccessor.HttpContext?.User.FindFirstValue(nameof(FirstName)) ?? string.Empty;
    public string LastName => _httpContextAccessor.HttpContext?.User.FindFirstValue(nameof(LastName)) ?? string.Empty;
}