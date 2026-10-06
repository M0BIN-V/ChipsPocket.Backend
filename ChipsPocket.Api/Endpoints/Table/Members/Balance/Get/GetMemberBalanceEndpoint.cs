using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Api.Endpoints.Table.Members.Balance.Get;

public sealed record GetMemberBalanceResponse(int Value);

public class GetMemberBalanceEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("", async Task<Results<
                NotFound<string>,
                Ok<GetMemberBalanceResponse>>> (
                Guid tableId,
                string memberId,
                IActiveHandRepository activeHandRepo,
                IMembersRepository membersRepository) =>
            {
                var hand = activeHandRepo.GetHand(tableId);
                if (hand is not null)
                {
                    var member = hand.Seats.SingleOrDefault(s => s.Player.UserId == memberId)?.Player;

                    if (member is null) return NotFound("member not found");

                    return Ok(new GetMemberBalanceResponse(member.Stack));
                }
                else
                {
                    var member = await membersRepository.GetTableMemberAsync(tableId, memberId);
                    if (member is null) return NotFound("member not found");
                    return Ok(new GetMemberBalanceResponse(member.Stack));
                }
            })
            .WithName("GetMemberBalance")
            .WithSummary("Get a table member's balance")
            .WithDescription(
                "Returns the current balance of a member at the specified table.")
            .RequireAuthorization();
    }
}