using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Endpoints.Auth;

public sealed record RegisterRequest(string Username, string Password);

public static class RegisterEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("/register", Handle)
            .WithSummary("Register a new user")
            .WithDescription("""
                             Creates a new user account using a username and password.

                             The username must not already be in use. If the provided credentials
                             do not satisfy the configured identity requirements, the validation
                             errors are returned to the client.
                             """);
    }

    private static async Task<Results<Ok, Conflict<string>, ValidationProblem>>
        Handle(RegisterRequest request, UserManager<User> userManager)
    {
        var existingUser = await userManager.FindByNameAsync(request.Username);

        if (existingUser is not null) return Conflict("Username is already taken.");

        var user = new User
        {
            UserName = request.Username
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .GroupBy(x => x.Code)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(e => e.Description).ToArray());

            return ValidationProblem(errors);
        }

        return Ok();
    }
}