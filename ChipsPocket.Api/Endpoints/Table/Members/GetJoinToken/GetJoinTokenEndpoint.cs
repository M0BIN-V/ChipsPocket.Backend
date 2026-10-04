using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Api.Endpoints.Table.Members.GetJoinToken;

public record GetJoinTokenResponse(string Token);

public class GetJoinTokenEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("join-token", async Task<Results<
                Ok<GetJoinTokenResponse>,
                ForbidHttpResult,
                NotFound<string>>> (
                [FromRoute] Guid tableId,
                [FromServices] ITableJoinTokenService tokenService,
                [FromServices] ITableRepository tableRepository,
                [FromServices] IMemberService memberService,
                [FromServices] ICurrentUser currentUser) =>
            {
                var table = await tableRepository.GetTableAsync(tableId);
                if (table is null) return NotFound("table not found");

                var isUserMemberOfTable = await memberService.IsMemberOfTableAsync(tableId, currentUser.Id);

                if (!isUserMemberOfTable) return Forbid();

                var token = tokenService.Create(tableId);

                return Ok(new GetJoinTokenResponse(token));
            })
            .WithSummary("Get table join token")
            .WithDescription("""
                             Generates a short-lived token that can be shared with other users
                             to join the table members.
                             """)
            .RequireAuthorization();
    }
}