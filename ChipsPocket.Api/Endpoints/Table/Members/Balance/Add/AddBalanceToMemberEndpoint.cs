using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Domain.Contracts;

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
                AddBalanceRequest request,
                string memberId,
                Guid tableId,
                IMembersRepository membersRepository,
                ITableRepository tableRepository,
                IActiveHandRepository activeHandsRepo,
                AppDbContext db,
                ICurrentUser currentUser) =>
            {
                if (activeHandsRepo.GetHand(tableId) is not null) return Forbid();

                var managerId = await tableRepository.GetManagerIdAsync(tableId);
                if (managerId != currentUser.Id) return Forbid();

                var member = await membersRepository.GetTableMemberAsync(tableId, memberId);

                if (member is null) return NotFound("destination user not found");

                member.Stack += request.Value;

                await db.SaveChangesAsync();
                return Ok();
            })
            .WithName("AddMemberBalance")
            .WithSummary("Add balance to a table member")
            .WithDescription(
                "Adds the specified amount to a table member's balance. " +
                "Only the manager of the table can perform this operation.")
            .Validate<AddBalanceRequest>()
            .RequireAuthorization();
    }
}