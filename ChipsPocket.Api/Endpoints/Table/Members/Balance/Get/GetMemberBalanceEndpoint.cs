namespace ChipsPocket.Api.Endpoints.Table.Members.Balance.Get;

public sealed record GetMemberBalanceResponse(int Value);

public class GetMemberBalanceEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("", async Task<Results<
                NotFound<string>,
                Ok<GetMemberBalanceResponse>>> (
                [FromRoute] Guid tableId,
                [FromRoute] string memberId,
                [FromServices] IUserStackService userStackService,
                CancellationToken cancellationToken) =>
            {
                var balance = await userStackService.GetBalanceAsync(
                    tableId,
                    memberId,
                    cancellationToken);

                return TypedResults.Ok(new GetMemberBalanceResponse(balance));
            })
            .WithName("GetMemberBalance")
            .WithSummary("Get a table member's balance")
            .WithDescription(
                "Returns the current balance of a member at the specified table.")
            .RequireAuthorization();
    }
}