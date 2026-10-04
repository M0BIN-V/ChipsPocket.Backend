using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Services.Shop;

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
                IShopService shopService,
                IMembersRepository membersRepository,
                ITableRepository tableRepository,
                AppDbContext db,
                ICurrentUser currentUser) =>
            {
                var managerId = await tableRepository.GetManagerIdAsync(tableId);
                if (managerId != currentUser.Id) return Forbid();

                if (!await membersRepository.IsMemberOfTableAsync(tableId, memberId))
                    return NotFound("destination user not found");

                shopService.BuyChipsAsync(tableId, memberId, request.Value);

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