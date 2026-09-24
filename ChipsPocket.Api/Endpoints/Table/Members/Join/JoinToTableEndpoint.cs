using ChipsPocket.Api.Notifications.Table;

namespace ChipsPocket.Api.Endpoints.Table.Players.Join;

public record JoinResponse(Guid TableId);

public class JoinToTableEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("join/{token}", async Task<Results<
                UnauthorizedHttpResult,
                Ok<JoinResponse>,
                Conflict,
                NotFound<string>>> (
                [FromRoute] string token,
                [FromServices] ITableNotificationPublisher publisher,
                [FromServices] ITableJoinTokenService tokenService,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                if (!tokenService.TryGetTableId(token.ToUpper(), out var tableId))
                    return TypedResults.NotFound("table not found");

                var user = await db.Users.SingleOrDefaultAsync(u => u.Id == currentUser.Id);

                if (user is null) return TypedResults.Unauthorized();

                var table = await db.Tables
                    .Include(t => t.Members)
                    .SingleOrDefaultAsync(t => t.Id == tableId);

                if (table is null) return TypedResults.NotFound("table not found");

                if (table.Members.Count == 10)
                    return TypedResults.Conflict();

                if (table.Members.All(u => u.UserId != user.Id))
                    table.Members.Add(new TableMember
                    {
                        UserId = user.Id,
                        TableId = tableId
                    });

                await db.SaveChangesAsync();

                var notification = new MemberJoinedToTableNotification(user.Id, user.UserName!);

                await publisher.PublishAsync(tableId, notification);

                return TypedResults.Ok(new JoinResponse(tableId));
            })
            .WithSummary("Join to table players")
            .WithDescription("""
                             Adds the authenticated user to a table using a join token.

                             If the user is already a member of the table, no duplicate membership
                             is created.

                             The join token must be valid and correspond to an existing table.

                             Only 10 users can be joined
                             """)
            .RequireAuthorization();
    }
}