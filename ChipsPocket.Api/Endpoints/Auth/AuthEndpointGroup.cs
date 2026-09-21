namespace ChipsPocket.Api.Endpoints.Auth;

public static class AuthEndpointGroup
{
    public static IEndpointRouteBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/auth")
            .WithTags("Authentication");

        RegisterEndpoint.Map(group);
        LoginEndpoint.Map(group);
        MeEndpoint.Map(group);

        return endpoints;
    }
}