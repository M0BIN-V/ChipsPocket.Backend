namespace ChipsPocket.Api.Endpoints.Auth;

public sealed record LoginRequest(
    string Username,
    string Password);

public static class LoginEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("/login", Handle)
            .WithSummary("Log in")
            .WithDescription("""
                             Authenticates a user using their username and password.

                             If the credentials are valid, a JWT access token is returned.
                             The same unauthorized response is returned when the username does not exist
                             or the password is incorrect.
                             """)
            .WithTags("Authentication");
    }

    private static async Task<Results<Ok<AuthResponse>, UnauthorizedHttpResult>>
        Handle(LoginRequest request, UserManager<User> userManager, JwtTokenService tokenService)
    {
        var user = await userManager.FindByNameAsync(request.Username);

        if (user is null) return TypedResults.Unauthorized();

        var validPassword = await userManager.CheckPasswordAsync(user, request.Password);

        if (!validPassword) return TypedResults.Unauthorized();

        var response = tokenService.CreateToken(user);

        return TypedResults.Ok(response);
    }
}