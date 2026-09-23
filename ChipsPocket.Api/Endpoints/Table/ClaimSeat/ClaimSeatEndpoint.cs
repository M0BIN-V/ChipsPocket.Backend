using ChipsPocket.Api.Abstractions.Endpionts;
using Microsoft.AspNetCore.Mvc;

namespace ChipsPocket.Api.Endpoints.Table.ClaimSeat;

public class ClaimSeatEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("{tableId:guid}/seats/{seatId:guid}/claim",
                async Task<Results<
                    ForbidHttpResult,
                    NotFound,
                    Conflict<string>,
                    Ok>> (
                    [FromRoute] Guid tableId,
                    [FromRoute] Guid seatId,
                    [FromServices] AppDbContext db,
                    [FromServices] ICurrentUser currentUser) =>
                {
                    var isLobbyMember = await db.Tables
                        .AnyAsync(t =>
                            t.Id == tableId &&
                            t.Lobby.LobbyUsers.Any(x =>
                                x.UserId == currentUser.Id));

                    if (!isLobbyMember) return TypedResults.Forbid();

                    var newSeat = await db.Seats
                        .FirstOrDefaultAsync(x => x.Id == seatId && x.TableId == tableId);

                    if (newSeat is null) return TypedResults.NotFound();

                    if (newSeat.UserId is not null && newSeat.UserId != currentUser.Id)
                        return TypedResults.Conflict("This seat is already occupied.");

                    // Find the user's current seat, if any.
                    var currentSeat = await db.Seats.FirstOrDefaultAsync(x =>
                        x.TableId == tableId &&
                        x.UserId == currentUser.Id);

                    // Already sitting on this seat.
                    if (currentSeat?.Id == newSeat.Id) return TypedResults.Ok();

                    // Leave the previous seat.
                    currentSeat?.UserId = null;

                    // Claim the new seat.
                    newSeat.UserId = currentUser.Id;

                    await db.SaveChangesAsync();

                    return TypedResults.Ok();
                })
            .WithSummary("Claim a table seat")
            .WithDescription("""
                             Claims a seat at a poker table for the authenticated user.

                             The user must be a member of the table's lobby.

                             If the user is already sitting at another seat at the same
                             table, their previous seat is released and the requested
                             seat becomes their new seat.

                             The requested seat must belong to the specified table and
                             must not already be occupied by another user.
                             """)
            .RequireAuthorization();
    }
}