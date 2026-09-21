namespace ChipsPocket.Api.Endpoints;

public sealed record LoginRequest(
    string Username,
    string Password);

public sealed record RegisterRequest(
    string Username,
    string Password);

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth");

        group.MapPost("/register", Register);

        group.MapPost("/login", Login);

        return endpoints;
    }

    private static async Task<IResult> Register(
        RegisterRequest request,
        UserManager<User> userManager)
    {
        var existingUser = await userManager.FindByNameAsync(request.Username);

        if (existingUser is not null)
            return Results.Conflict(new
            {
                message = "Username is already taken."
            });

        var user = new User
        {
            UserName = request.Username
        };

        var result = await userManager.CreateAsync(
            user,
            request.Password);

        if (!result.Succeeded)
            return Results.BadRequest(new
            {
                errors = result.Errors.Select(x => new
                {
                    x.Code,
                    x.Description
                })
            });

        return Results.Ok();
    }

    private static async Task<IResult> Login(
        LoginRequest request,
        UserManager<User> userManager,
        JwtTokenService tokenService)
    {
        var user = await userManager.FindByNameAsync(
            request.Username);

        if (user is null) return Results.Unauthorized();

        var validPassword = await userManager.CheckPasswordAsync(
            user,
            request.Password);

        if (!validPassword) return Results.Unauthorized();

        var response = tokenService.CreateToken(user);

        return Results.Ok(response);
    }
}