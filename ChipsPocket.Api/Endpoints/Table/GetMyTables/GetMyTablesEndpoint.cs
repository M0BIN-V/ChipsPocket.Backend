namespace ChipsPocket.Api.Endpoints.Table.GetMyTables;

public class GetMyTablesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("my", async Task<Ok<List<GetMyTablesResponse>>> (
                ICurrentUser currentUser,
                AppDbContext db,
                CancellationToken cancellationToken) =>
            {
                var userId = currentUser.Id;

                var lobbies = await db.Set<LobbyUser>()
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Select(x => new GetMyTablesResponse(
                        x.Lobby.TableId,
                        x.Lobby.Table.Name,
                        x.Lobby.Table.CreatedAt
                    ))
                    .ToListAsync(cancellationToken);

                lobbies = lobbies.OrderByDescending(t => t.CreatedAt).ToList();

                return TypedResults.Ok(lobbies);
            })
            .WithName("GetMyTables")
            .WithSummary("Get tables the current user has joined")
            .WithDescription(
                """
                Returns a list of poker tables where the currently authenticated user
                is a member of the table's lobby.

                Only tables associated with the current user's lobby memberships are returned.
                """)
            .RequireAuthorization();
    }
}

public record GetMyTablesResponse(Guid TableId, string TableName, DateTimeOffset CreatedAt);