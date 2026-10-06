using ChipsPocket.Api.Infra.Persistence;
using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Api.Endpoints.Table.Members.Balance.Deduct;

public class DeductMemberBalanceEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapDelete("", async Task<Results<
                ForbidHttpResult,
                NotFound<string>,
                BadRequest<string>,
                Ok>> (
                [FromQuery] int value,
                string memberId,
                Guid tableId,
                AppDbContext db,
                IActiveHandRepository activeHandsRepo,
                IMembersRepository membersRepository,
                ICurrentUser currentUser) =>
            {
                if (activeHandsRepo.GetHand(tableId) is not null) return Forbid();

                if (value < 0) return BadRequest("value must be positive");

                var userIsTableManager = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.ManagerId == currentUser.Id);

                if (!userIsTableManager) return Forbid();

                var member = await membersRepository.GetTableMemberAsync(tableId, memberId);

                if (member is null) return NotFound("source user not found");

                var userHasEnoughChips = member.Stack >= value;

                if (!userHasEnoughChips) return BadRequest("user does not have enough balance");

                member.Stack -= value;

                await db.SaveChangesAsync();

                return Ok();
            })
            .WithName("DeductMemberBalance")
            .WithSummary("Deduct balance from a table member")
            .WithDescription(
                "Deducts the specified amount from a table member's balance. " +
                "Only the managerService of the table can perform this operation. " +
                "The member must have sufficient balance.")
            .RequireAuthorization();
    }
}