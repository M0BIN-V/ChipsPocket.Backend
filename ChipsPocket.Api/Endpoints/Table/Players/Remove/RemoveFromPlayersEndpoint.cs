namespace ChipsPocket.Api.Endpoints.Table.Lobby.LeftTable;

public class RemoveFromPlayersEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapDelete(
                "{tableId:guid}/lobby/{removeUserId}",
                async Task<Results<
                    ForbidHttpResult,
                    NotFound<string>,
                    Ok>> (
                    [FromRoute] Guid tableId,
                    [FromRoute] string removeUserId,
                    [FromServices] AppDbContext db,
                    [FromServices] ICurrentUser currentUser) =>
                {
                    var table = await db.Tables
                        .Include(t => t.Members)
                        .SingleOrDefaultAsync(t => t.Id == tableId);

                    if (table is null)
                        return TypedResults.NotFound("Table not found");

                    var isManager = table.ManagerId == currentUser.Id;
                    var removingSelf = removeUserId == currentUser.Id;

                    if (!isManager && !removingSelf) return TypedResults.Forbid();

                    if (isManager && removingSelf) return TypedResults.Forbid();

                    var lobbyUser = table.Members
                        .SingleOrDefault(u => u.UserId == removeUserId);

                    if (lobbyUser is null)
                        return TypedResults.NotFound("User is not in the lobby");

                    var claimedSeat = await db.Seats
                        .SingleOrDefaultAsync(s =>
                            s.TableId == tableId &&
                            s.UserId == removeUserId);

                    if (claimedSeat is not null)
                        claimedSeat.UserId = null;

                    table.Members.Remove(lobbyUser);

                    await db.SaveChangesAsync();

                    return TypedResults.Ok();
                })
            .WithSummary("Remove a user from a table")
            .WithDescription("""
                             Removes a user from the specified table.

                             A table manager can remove any user from the table except themselves.
                             A regular user can only remove themselves from the table players.

                             If the user has a claimed seat at the table, the seat is released
                             when the user is removed from the table players.

                             The operation requires authentication and the specified table must exist.
                             """)
            .RequireAuthorization();
    }
}