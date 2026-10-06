using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Api.Notifications.Table;
using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Api.Endpoints.Table.ReleaseSeat;

public class ReleaseSeatEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("{tableId:guid}/seats/{seatId:guid}/release", async Task<Results<
                ForbidHttpResult,
                NotFound,
                Ok>> (
                Guid tableId,
                Guid seatId,
                ITableNotificationPublisher publisher,
                AppDbContext db,
                IMembersRepository membersRepository,
                IActiveHandRepository activeHandRepo,
                ICurrentUser currentUser) =>
            {
                if (!await membersRepository.IsMemberOfTableAsync(tableId, currentUser.Id) ||
                    activeHandRepo.GetHand(tableId) is not null)
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