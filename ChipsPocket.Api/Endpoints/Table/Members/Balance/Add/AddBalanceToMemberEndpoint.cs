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
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var userIsTableManager = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.ManagerId == currentUser.Id);

                if (!userIsTableManager) return TypedResults.Forbid();

                var destinationUserIsTableMember = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.Members
                        .Any(u => u.UserId == memberId));

                if (!destinationUserIsTableMember) return TypedResults.NotFound("destination user not found");

                var transaction = TransactionBuilder
                    .Create(tableId)
                    .AddValue(request.Value)
                    .ToUser(memberId)
                    .FromShop()
                    .Build();

                await db.ChipTransactions.AddAsync(transaction);
                await db.SaveChangesAsync();
                return TypedResults.Ok();
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