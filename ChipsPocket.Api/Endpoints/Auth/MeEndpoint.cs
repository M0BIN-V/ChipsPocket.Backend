using System.Security.Claims;

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
            .RequireAuthorization();
    }

    private static async Task<Results<Ok<MeResponse>, UnauthorizedHttpResult>>
        Handle(ClaimsPrincipal principal, UserManager<User> userManager)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null) return TypedResults.Unauthorized();

        var user = await userManager.FindByIdAsync(userId);

        if (user is null) return TypedResults.Unauthorized();

        return TypedResults.Ok(new MeResponse(user.Id, user.UserName!, user.Email));
    }
}