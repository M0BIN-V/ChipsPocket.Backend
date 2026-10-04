using ChipsPocket.Api.Notifications.Table;
using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Endpoints.Table.ReleaseSeat;

public class ReleaseSeatEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("{tableId:guid}/seats/{seatId:guid}/release", async Task<Results<
                ForbidHttpResult,
                NotFound,
                Ok>> (
                [FromRoute] Guid tableId,
                [FromRoute] Guid seatId,
                [FromServices] ITableNotificationPublisher publisher,
                [FromServices] AppDbContext db,
                [FromServices] IMemberService memberService,
                [FromServices] IHandRepository handRepository,
                [FromServices] ITableRepository tableRepository,
                [FromServices] ICurrentUser currentUser) =>
            {
                if (!await memberService.IsMemberOfTableAsync(tableId, currentUser.Id))
                    return Forbid();

                var lastHand = await handRepository.GetLastHandAsync(tableId);
                if (lastHand is not null && lastHand.CurrentStreet != Street.Finished)
                    return Forbid();

                var seat = await db.Seats
                    .FirstOrDefaultAsync(x => x.Id == seatId && x.TableId == tableId);

                if (seat is null) return NotFound();

                // The seat is already free.
                if (seat.UserId is null) return Ok();

                // A user can only release their own seat.
                if (seat.UserId != currentUser.Id) return Forbid();

                seat.UserId = null;

                await db.SaveChangesAsync();

                await publisher.PublishAsync(tableId, new MemberReleasedSeatNotification(currentUser.Id, seatId));

                return Ok();
            })
            .WithSummary("Release a table seat")
            .WithDescription("""
                             Releases a seat currently occupied by the authenticated user.

                             The user must be a member of the table players and can only
                             release a seat that they currently occupy.

                             If the seat is already empty, the operation succeeds without
                             making any changes.
                             """)
            .RequireAuthorization();
    }
}