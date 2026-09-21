using System.Security.Claims;

namespace ChipsPocket.Api.Services;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor)
    : ICurrentUser
{
    public string Id =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(
            ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("User is not authenticated.");
}