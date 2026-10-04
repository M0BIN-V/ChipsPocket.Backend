using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Api.Notifications.Table;
using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Api.Endpoints.Table.Members.Join;

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
                [FromServices] IMembersRepository membersRepository,
                [FromServices] ITableRepository tableRepository,
                [FromServices] ITableNotificationPublisher publisher,
                [FromServices] ITableJoinTokenService tokenService,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                if (!tokenService.TryGetTableId(token.ToUpper(), out var tableId))
                    return NotFound("table not found");

                var user = await db.Users.SingleOrDefaultAsync(u => u.Id == currentUser.Id);

                if (user is null) return Unauthorized();

                var table = await tableRepository.GetTableAsync(tableId);

                if (table is null) return NotFound("table not found");

                var members = await membersRepository.GetTableMembersAsync(tableId);

                if (members.Count == 10) return Conflict();

                if (members.All(u => u.UserId != user.Id)) membersRepository.AddMember(tableId, user.Id);

                await db.SaveChangesAsync();

                var notification = new MemberJoinedToTableNotification(user.Id, user.UserName!);

                await publisher.PublishAsync(tableId, notification);

                return Ok(new JoinResponse(tableId));
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