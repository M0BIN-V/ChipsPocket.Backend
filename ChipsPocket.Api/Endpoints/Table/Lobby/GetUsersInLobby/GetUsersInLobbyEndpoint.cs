using ChipsPocket.Api.Abstractions.Endpionts;
using Microsoft.AspNetCore.Mvc;

namespace ChipsPocket.Api.Endpoints.Table.Lobby.GetUsersInLobby;

public record LobbyUserDto(string Id, string Username);

public class GetUsersInLobbyEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("{tableId:guid}", async Task<Results<
                ForbidHttpResult,
                Ok<IEnumerable<LobbyUserDto>>>> (
                [FromRoute] Guid tableId,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var isMember = await db.Tables
                    .AnyAsync(t =>
                        t.Id == tableId &&
                        t.Lobby.LobbyUsers.Any(u => u.UserId == currentUser.Id));

                if (!isMember) return TypedResults.Forbid();

                var users = await db.Tables
                    .Where(t => t.Id == tableId)
                    .SelectMany(t => t.Lobby.LobbyUsers)
                    .Select(x => new LobbyUserDto(
                        x.User.Id,
                        x.User.UserName!))
                    .ToListAsync();

                return TypedResults.Ok<IEnumerable<LobbyUserDto>>(users);
            })
            .WithSummary("Get lobby users")
            .WithDescription("""
                             Returns the users currently in the lobby of the specified table.

                             Only users who are members of the table's lobby can access
                             the list of lobby users.
                             """)
            .RequireAuthorization();
    }
}