namespace ChipsPocket.Api.Endpoints.Table.CashOut;

public record CashOutRequest(
    string SourceUserId,
    Guid ChipId,
    int ChipCount);

public class CashOutRequestValidator : AbstractValidator<CashOutRequest>
{
    public CashOutRequestValidator()
    {
        RuleFor(r => r.SourceUserId)
            .NotEmpty();

        RuleFor(r => r.ChipId)
            .NotEmpty();

        RuleFor(r => r.ChipCount)
            .GreaterThan(0);
    }
}

public class CashOutEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("{tableId:guid}/cash-out", async Task<Results<
                ForbidHttpResult,
                NotFound<string>,
                BadRequest<string>,
                Ok>> (
                [FromBody] CashOutRequest request,
                [FromRoute] Guid tableId,
                [FromServices] AppDbContext db,
                [FromServices] IUserStackService userStackService,
                [FromServices] ICurrentUser currentUser) =>
            {
                var userIsTableManager = await db.Tables
                    .AnyAsync(t =>
                        t.Id == tableId &&
                        t.ManagerId == currentUser.Id);

                if (!userIsTableManager)
                    return TypedResults.Forbid();

                var sourceUserIsMember = await db.Tables
                    .AnyAsync(t =>
                        t.Id == tableId &&
                        t.Members
                            .Any(u => u.UserId == request.SourceUserId));

                if (!sourceUserIsMember)
                    return TypedResults.NotFound("source user not found");

                if (!await db.Chips.AnyAsync(c => c.Id == request.ChipId))
                    return TypedResults.NotFound("chip not found");

                var userStack = await userStackService.GetAsync(tableId, request.SourceUserId);

                var userHasEnoughChips =
                    userStack!.Chips.Any(c => c.ChipId == request.ChipId && c.Count >= request.ChipCount);

                if (!userHasEnoughChips)
                    return TypedResults.BadRequest("user does not have enough chips");

                var transaction = ChipTransactionBuilder
                    .Create(tableId)
                    .AddChip(request.ChipId, request.ChipCount)
                    .FromUser(request.SourceUserId)
                    .ToShop()
                    .Build();

                await db.ChipTransactions.AddAsync(transaction);
                await db.SaveChangesAsync();

                return TypedResults.Ok();
            })
            .WithSummary("Create a cash-out transaction")
            .WithDescription("""
                             Creates a cash-out transaction that transfers chips
                             from a user to the table shop.

                             user should have enough chips to cash-out, otherwise it will return a bad request.

                             Only the table manager can perform a cash-out.
                             The source user must currently be a member of the
                             table.
                             The specified chip must exist.
                             """)
            .Validate<CashOutRequest>()
            .RequireAuthorization();
    }
}