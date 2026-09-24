namespace ChipsPocket.Api.Endpoints.Table.Players.GetPlayers;

public record PlayerDto(string Id, string Username);

public class GetPlayersEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("{tableId:guid}", async Task<Results<
                ForbidHttpResult,
                Ok<IEnumerable<PlayerDto>>>> (
                [FromRoute] Guid tableId,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var isMember = await db.Tables
                    .AnyAsync(t =>
                        t.Id == tableId &&
                        t.Members.Any(u => u.UserId == currentUser.Id));

                if (!isMember) return TypedResults.Forbid();

                var users = await db.Tables
                    .Where(t => t.Id == tableId)
                    .SelectMany(t => t.Members)
                    .Select(x => new PlayerDto(
                        x.User.Id,
                        x.User.UserName!))
                    .ToListAsync();

                return TypedResults.Ok<IEnumerable<PlayerDto>>(users);
            })
            .WithSummary("Get table players")
            .WithDescription("""
                             Returns the users currently in the specified table.

                             Only users who are members of the table can access
                             the list of players.
                             """)
            .RequireAuthorization();
    }
}