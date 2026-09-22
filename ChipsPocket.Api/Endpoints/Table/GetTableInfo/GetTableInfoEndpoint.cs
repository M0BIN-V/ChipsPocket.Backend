using ChipsPocket.Api.Abstractions.Endpionts;
using Microsoft.AspNetCore.Mvc;

namespace ChipsPocket.Api.Endpoints.Table.GetTableInfo;

public record ViewUserDto(string Username);

public record ViewSeatDto(Guid Id, int Order, ViewUserDto? User);

public record ViewTableInfoDto(Guid Id, string Name, IEnumerable<ViewSeatDto> Seats);

public class GetTableInfoEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("{tableId:guid}", async Task<Results<
                ForbidHttpResult,
                Ok<ViewTableInfoDto>>> (
                [FromRoute] Guid tableId,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var userIsInTableLobby = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.Lobby.LobbyUsers
                        .Any(u => u.User.Id == currentUser.Id));

                if (!userIsInTableLobby) return TypedResults.Forbid();

                var table = await db.Tables
                    .Where(t => t.Id == tableId)
                    .Select(t => new ViewTableInfoDto(
                        t.Id,
                        t.Name,
                        t.Seats.Select(s => new ViewSeatDto(
                            s.Id,
                            s.Order,
                            s.User == null
                                ? null
                                : new ViewUserDto(s.User.UserName!)
                        ))
                    ))
                    .SingleAsync();

                return TypedResults.Ok(table);
            })
            .WithSummary("Get table information")
            .WithDescription("""
                Returns the details of a poker table, including its name and seats.

                The authenticated user must be a member of the table's lobby.
                Users who are not members of the lobby are forbidden from accessing
                the table information.
                """)
            .RequireAuthorization();
    }
}