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
                [FromBody] DeductMemberBalanceRequest request,
                [FromRoute] string memberId,
                [FromRoute] Guid tableId,
                [FromServices] AppDbContext db,
                [FromServices] IUserStackService userStackService,
                [FromServices] ICurrentUser currentUser) =>
            {
                var userIsTableManager = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.ManagerId == currentUser.Id);

                if (!userIsTableManager) return TypedResults.Forbid();

                var userIsTableMember = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.Members
                        .Any(u => u.UserId == memberId));

                if (!userIsTableMember) return TypedResults.NotFound("source user not found");

                var memberBalance = await userStackService.GetBalanceAsync(tableId, memberId);

                var userHasEnoughChips = memberBalance >= request.Value;

                if (!userHasEnoughChips) return TypedResults.BadRequest("user does not have enough balance");

                var transaction = TransactionBuilder
                    .Create(tableId)
                    .AddValue(request.Value)
                    .FromUser(memberId)
                    .ToShop()
                    .Build();

                await db.ChipTransactions.AddAsync(transaction);
                await db.SaveChangesAsync();

                return TypedResults.Ok();
            })
            .WithName("DeductMemberBalance")
            .WithSummary("Deduct balance from a table member")
            .WithDescription(
                "Deducts the specified amount from a table member's balance. " +
                "Only the manager of the table can perform this operation. " +
                "The member must have sufficient balance.")
            .Validate<DeductMemberBalanceRequest>()
            .RequireAuthorization();
    }
}