using System.Security.Claims;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Endpoints.Auth;

public sealed record MeResponse(
    string Id,
    string Username,
    string? Email);

public static class MeEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/me", Handle)
            .WithSummary("Get current user")
            .WithDescription("""
                             Returns the profile information of the currently authenticated user.

                             The user is identified using the NameIdentifier claim from the
                             authenticated user's access token.
                             """)
            .RequireAuthorization();
    }

    private static async Task<Results<Ok<MeResponse>, UnauthorizedHttpResult>>
        Handle(ClaimsPrincipal principal, UserManager<User> userManager)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null) return Unauthorized();

        var user = await userManager.FindByIdAsync(userId);

        if (user is null) return Unauthorized();

        return Ok(new MeResponse(user.Id, user.UserName!, user.Email));
    }
}