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

                var tables = await db.Tables
                    .Where(t => t.Members.Any(p => p.UserId == userId))
                    .Select(t => new GetMyTablesResponse(
                        t.Id,
                        t.Name,
                        t.CreatedAt
                    )).ToListAsync(cancellationToken);

                tables = tables.OrderByDescending(t => t.CreatedAt).ToList();

                return TypedResults.Ok(tables);
            })
            .WithName("GetMyTables")
            .WithSummary("Get tables the current user has joined")
            .WithDescription(
                """
                Returns a list of poker tables where the currently authenticated user
                is a member of the table.

                Only tables associated with the current user's memberships are returned.
                """)
            .RequireAuthorization();
    }
}

public record GetMyTablesResponse(Guid TableId, string TableName, DateTimeOffset CreatedAt);