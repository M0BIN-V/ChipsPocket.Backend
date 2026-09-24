namespace ChipsPocket.Api.Endpoints.Table.Players.GetJoinToken;

public record GetJoinTokenResponse(string Token);

public class GetJoinTokenEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("{tableId:guid}/join-token", async Task<Results<
                Ok<GetJoinTokenResponse>,
                NotFound<string>>> (
                [FromRoute] Guid tableId,
                [FromServices] ITableJoinTokenService tokenService,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var exists = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.Members.Any(u => u.UserId == currentUser.Id));

                if (!exists) return TypedResults.NotFound("table not found");

                var token = tokenService.Create(tableId);

                return TypedResults.Ok(new GetJoinTokenResponse(token));
            })
            .WithSummary("Get table join token")
            .WithDescription("""
                             Generates a short-lived token that can be shared with other users
                             to join the table members.
                             """)
            .RequireAuthorization();
    }
}