using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Endpoints.Table.Members.Balance.Add;

public record AddBalanceRequest(int Value);

public class AddBalanceRequestValidator : AbstractValidator<AddBalanceRequest>
{
    public AddBalanceRequestValidator()
    {
        RuleFor(r => r.Value)
            .GreaterThan(0);
    }
}

public class AddBalanceToMemberEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("", async Task<Results<
                ForbidHttpResult,
                NotFound<string>,
                Ok>> (
                [FromBody] AddBalanceRequest request,
                [FromRoute] string memberId,
                [FromRoute] Guid tableId,
                [FromServices] IMemberService memberService,
                [FromServices] ITableRepository tableRepository,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var managerId = await tableRepository.GetManagerIdAsync(tableId);
                if (managerId != currentUser.Id) return Forbid();

                if (!await memberService.IsMemberOfTableAsync(tableId, memberId))
                    return NotFound("destination user not found");

                var transaction = TransactionBuilder
                    .Create(tableId)
                    .WithAmount(request.Value)
                    .ToUser(memberId)
                    .FromShop()
                    .Build();

                await db.Transactions.AddAsync(transaction);
                await db.SaveChangesAsync();
                return Ok();
            })
            .WithName("AddMemberBalance")
            .WithSummary("Add balance to a table member")
            .WithDescription(
                "Adds the specified amount to a table member's balance. " +
                "Only the managerService of the table can perform this operation.")
            .Validate<AddBalanceRequest>()
            .RequireAuthorization();
    }
}