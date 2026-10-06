using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Api.Endpoints.Table.Members.Remove;

public class RemoveFromMembersEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapDelete("{memberId}", async Task<Results<
                ForbidHttpResult,
                NotFound<string>,
                Ok>> (
                Guid tableId,
                string memberId,
                ISeatRepository seatRepository,
                ITableRepository tableRepository,
                IMembersRepository membersRepository,
                IActiveHandRepository activeHandRepo,
                ICurrentUser currentUser) =>
            {
                if (activeHandRepo.GetHand(tableId) is not null)
                    return Forbid();

                var table = await tableRepository.GetTableAsync(tableId);
                if (table is null) return NotFound("Table not found");

                var isManager = table.ManagerId == currentUser.Id;
                var removingSelf = memberId == currentUser.Id;

                if (!isManager && !removingSelf) return Forbid();

                if (isManager && removingSelf) return Forbid();

                var tableMembers = await membersRepository.GetTableMembersAsync(tableId);

                if (tableMembers.All(m => m.UserId != memberId))
                    return NotFound("User is not in the members");

                await seatRepository.ReleaseSeatAsync(tableId, memberId);
                await membersRepository.RemoveAsync(tableId, memberId);

                return Ok();
            })
            .WithSummary("Remove a user from a table")
            .WithDescription("""
                             Removes a user from the specified table.

                             A table managerService can remove any user from the table except themselves.
                             A regular user can only remove themselves from the table players.

                             If the user has a claimed seat at the table, the seat is released
                             when the user is removed from the table players.

                             The operation requires authentication and the specified table must exist.
                             """)
            .RequireAuthorization();
    }
}