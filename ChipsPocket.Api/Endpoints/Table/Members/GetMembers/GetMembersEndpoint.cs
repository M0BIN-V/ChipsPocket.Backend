namespace ChipsPocket.Api.Endpoints.Table.Members.GetMembers;

public record MemberDto(string Id, string Username);

public class GetMembersEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("", async Task<Results<
                ForbidHttpResult,
                Ok<IEnumerable<MemberDto>>>> (
                [FromRoute] Guid tableId,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var isMember = await db.Tables
                    .AnyAsync(t =>
                        t.Id == tableId &&
                        t.Members.Any(u => u.UserId == currentUser.Id));

                if (!isMember) return TypedResults.Forbid();

                var users = await db.Tables
                    .Where(t => t.Id == tableId)
                    .SelectMany(t => t.Members)
                    .Select(x => new MemberDto(
                        x.User.Id,
                        x.User.UserName!))
                    .ToListAsync();

                return TypedResults.Ok<IEnumerable<MemberDto>>(users);
            })
            .WithSummary("Get table players")
            .WithDescription("""
                             Returns the users currently in the specified table.

                             Only users who are members of the table can access
                             the list of players.
                             """)
            .RequireAuthorization();
    }
}