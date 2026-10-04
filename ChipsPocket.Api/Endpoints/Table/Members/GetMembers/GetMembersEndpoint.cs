using ChipsPocket.Domain.Contracts;

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
                [FromServices] IUserRepository userRepository,
                [FromServices] IMembersRepository membersRepository,
                [FromServices] ICurrentUser currentUser) =>
            {
                var isMember = await membersRepository.IsMemberOfTableAsync(tableId, currentUser.Id);

                if (!isMember) return Forbid();

                var members = await membersRepository.GetTableMembersAsync(tableId);
                var memberUserIds = members.Select(m => m.UserId)
                    .ToList();

                var users = await userRepository.GetUsersAsync(memberUserIds);

                var response = users
                    .Select(u => new MemberDto(u.Id, u.UserName!));

                return Ok(response);
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