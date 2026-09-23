using ChipsPocket.Api.Notifications.Table;

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
                    [FromServices] ITableNotificationPublisher publisher,
                    [FromServices] AppDbContext db,
                    [FromServices] ICurrentUser currentUser) =>
                {
                    var user = await db.Users.SingleAsync(u => u.Id == currentUser.Id);

                    var isLobbyMember = await db.Tables
                        .AnyAsync(t =>
                            t.Id == tableId &&
                            t.Lobby.LobbyUsers.Any(x =>
                                x.UserId == user.Id));


                    if (!isLobbyMember) return TypedResults.Forbid();

                    var newSeat = await db.Seats
                        .FirstOrDefaultAsync(x => x.Id == seatId && x.TableId == tableId);

                    if (newSeat is null) return TypedResults.NotFound();

                    if (newSeat.UserId is not null && newSeat.UserId != user.Id)
                        return TypedResults.Conflict("This seat is already occupied.");

                    // Find the user's current seat, if any.
                    var currentSeat = await db.Seats.FirstOrDefaultAsync(x =>
                        x.TableId == tableId &&
                        x.UserId == user.Id);

                    // Already sitting on this seat.
                    if (currentSeat?.Id == newSeat.Id) return TypedResults.Ok();

                    // Leave the previous seat.
                    currentSeat?.UserId = null;

                    // Claim the new seat.
                    newSeat.UserId = user.Id;

                    await db.SaveChangesAsync();

                    var notification = new PlayerClaimedSeatNotification(seatId, user.Id, user.UserName!);
                    await publisher.PublishAsync(tableId, notification);

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