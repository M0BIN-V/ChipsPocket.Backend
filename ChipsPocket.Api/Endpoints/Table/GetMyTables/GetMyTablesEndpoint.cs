using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Api.Endpoints.Table.GetMyTables;

public class GetMyTablesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("my", async Task<Ok<List<GetMyTablesResponse>>> (
                ICurrentUser currentUser,
                IMemberService memberService,
                ITableRepository tableRepository) =>
            {
                var userId = currentUser.Id;

                var userTableIds = await memberService.GetTableIdsAsync(userId);

                var tables = await tableRepository.GetTablesAsync(userTableIds);

                var response = tables
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(t => new GetMyTablesResponse(t.Id, t.Name, t.CreatedAt))
                    .ToList();

                return Ok(response);
            })
            .WithName("GetMyTables")
            .WithSummary("Get tables the current user has joined")
            .WithDescription(
                """
                Returns a list of poker tables where the currently authenticated user
                is a member of the table.

                Only tables associated with the current user's memberships are returned.
                """)
            .RequireAuthorization();
    }
}

public record GetMyTablesResponse(Guid TableId, string TableName, DateTimeOffset CreatedAt);