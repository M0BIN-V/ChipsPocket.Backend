using ChipsPocket.Api.Abstractions.Endpionts;
using ChipsPocket.Api.Notifications.Table;
using Microsoft.AspNetCore.Mvc;

namespace ChipsPocket.Api.Endpoints.Table.ReleaseSeat;

public class ReleaseSeatEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost(
                "{tableId:guid}/seats/{seatId:guid}/release",
                async Task<Results<
                    ForbidHttpResult,
                    NotFound,
                    Ok>> (
                    [FromRoute] Guid tableId,
                    [FromRoute] Guid seatId,
                    [FromServices] ITableNotificationPublisher publisher,
                    [FromServices] AppDbContext db,
                    [FromServices] ICurrentUser currentUser) =>
                {
                    var isLobbyMember = await db.Tables
                        .AnyAsync(t => t.Id == tableId && t.Lobby.LobbyUsers
                            .Any(x => x.UserId == currentUser.Id));

                    if (!isLobbyMember) return TypedResults.Forbid();

                    var seat = await db.Seats
                        .FirstOrDefaultAsync(x => x.Id == seatId && x.TableId == tableId);

                    if (seat is null) return TypedResults.NotFound();

                    // The seat is already free.
                    if (seat.UserId is null) return TypedResults.Ok();

                    // A user can only release their own seat.
                    if (seat.UserId != currentUser.Id) return TypedResults.Forbid();

                    seat.UserId = null;

                    await db.SaveChangesAsync();

                    await publisher.PublishAsync(tableId, new PlayerReleasedSeatNotification(currentUser.Id, seatId));

                    return TypedResults.Ok();
                })
            .WithSummary("Release a table seat")
            .WithDescription("""
                             Releases a seat currently occupied by the authenticated user.

                             The user must be a member of the table's lobby and can only
                             release a seat that they currently occupy.

                             If the seat is already empty, the operation succeeds without
                             making any changes.
                             """)
            .RequireAuthorization();
    }
}