using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Endpoints.Table.Members.Remove;

public class RemoveFromMembersEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapDelete("{memberId}",
                async Task<Results<
                    ForbidHttpResult,
                    NotFound<string>,
                    Ok>> (
                    [FromRoute] Guid tableId,
                    [FromRoute] string memberId,
                    [FromServices] ISeatRepository seatRepository,
                    [FromServices] ITableRepository tableRepository,
                    [FromServices] IMembersRepository membersRepository,
                    [FromServices] IHandRepository handRepository,
                    [FromServices] ICurrentUser currentUser) =>
                {
                    var lastHand = await handRepository.GetLastHandAsync(tableId);
                    if (lastHand is not null && lastHand.CurrentStreet != Street.Finished)
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