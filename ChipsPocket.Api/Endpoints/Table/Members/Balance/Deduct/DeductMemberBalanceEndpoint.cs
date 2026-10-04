using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Services.Shop;
using ChipsPocket.Domain.Services.UserStack;

namespace ChipsPocket.Api.Endpoints.Table.Members.Balance.Deduct;

public record DeductMemberBalanceRequest(int Value);

public class DeductMemberBalanceRequestValidator : AbstractValidator<DeductMemberBalanceRequest>
{
    public DeductMemberBalanceRequestValidator()
    {
        RuleFor(r => r.Value)
            .GreaterThan(0);
    }
}

public class DeductMemberBalanceEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapDelete("", async Task<Results<
                ForbidHttpResult,
                NotFound<string>,
                BadRequest<string>,
                Ok>> (
                DeductMemberBalanceRequest request,
                string memberId,
                Guid tableId,
                AppDbContext db,
                IShopService shopService,
                IMembersRepository membersRepository,
                IUserStackService userStackService,
                ICurrentUser currentUser) =>
            {
                var userIsTableManager = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.ManagerId == currentUser.Id);

                if (!userIsTableManager) return Forbid();

                var userIsTableMember = await membersRepository.IsMemberOfTableAsync(tableId, memberId);

                if (!userIsTableMember) return NotFound("source user not found");

                var memberBalance = await userStackService.GetBalanceAsync(tableId, memberId);

                var userHasEnoughChips = memberBalance >= request.Value;

                if (!userHasEnoughChips) return BadRequest("user does not have enough balance");

                shopService.SellChipsAsync(tableId, memberId, request.Value);

                await db.SaveChangesAsync();

                return Ok();
            })
            .WithName("DeductMemberBalance")
            .WithSummary("Deduct balance from a table member")
            .WithDescription(
                "Deducts the specified amount from a table member's balance. " +
                "Only the managerService of the table can perform this operation. " +
                "The member must have sufficient balance.")
            .Validate<DeductMemberBalanceRequest>()
            .RequireAuthorization();
    }
}