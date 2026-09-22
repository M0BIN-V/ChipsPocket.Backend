using ChipsPocket.Api.Abstractions.Endpionts;
using Microsoft.AspNetCore.Mvc;

namespace ChipsPocket.Api.Endpoints.Table.Lobby.Join;

public class JoinToLobbyEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("join/{token}", async Task<Results<
                UnauthorizedHttpResult,
                Ok,
                NotFound<string>>> (
                [FromRoute] string token,
                [FromServices] ITableJoinTokenService tokenService,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                if (!tokenService.TryGetTableId(token, out var tableId))
                    return TypedResults.NotFound("table not found");

                var user = await db.Users.SingleOrDefaultAsync(u => u.Id == currentUser.Id);

                if (user is null) return TypedResults.Unauthorized();

                var table = await db.Tables
                    .Include(t => t.Lobby)
                    .ThenInclude(l => l.LobbyUsers)
                    .SingleOrDefaultAsync(t => t.Id == tableId);

                if (table is null) return TypedResults.NotFound("table not found");

                var lobby = table.Lobby;

                if (lobby.LobbyUsers.All(u => u.UserId != user.Id))
                    lobby.LobbyUsers.Add(new LobbyUser
                    {
                        UserId = user.Id,
                        LobbyId = lobby.Id
                    });

                await db.SaveChangesAsync();

                return TypedResults.Ok();
            })
            .WithSummary("Join a table lobby")
            .WithDescription("""
                             Adds the authenticated user to a table's lobby using a join token.

                             If the user is already a member of the lobby, no duplicate membership
                             is created.

                             The join token must be valid and correspond to an existing table.
                             """)
            .RequireAuthorization();
    }
}