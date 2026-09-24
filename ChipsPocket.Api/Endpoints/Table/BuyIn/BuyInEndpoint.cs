namespace ChipsPocket.Api.Endpoints.Table.BuyIn;

public record BuyInRequest(string DestinationUserId, Guid ChipId, int ChipCount);

public class BuyInRequestValidator : AbstractValidator<BuyInRequest>
{
    public BuyInRequestValidator()
    {
        RuleFor(r => r.DestinationUserId)
            .NotEmpty();

        RuleFor(r => r.ChipId)
            .NotEmpty();

        RuleFor(r => r.ChipCount)
            .GreaterThan(0);
    }
}

public class BuyInEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("{tableId:guid}/buy-in", async Task<Results<
                ForbidHttpResult,
                NotFound<string>,
                Ok>> (
                [FromBody] BuyInRequest request,
                [FromRoute] Guid tableId,
                [FromServices] AppDbContext db,
                [FromServices] ICurrentUser currentUser) =>
            {
                var userIsTableManager = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.ManagerId == currentUser.Id);

                if (!userIsTableManager) return TypedResults.Forbid();

                var destinationUserIsTableMember = await db.Tables
                    .AnyAsync(t => t.Id == tableId && t.Members
                        .Any(u => u.UserId == request.DestinationUserId));

                if (!destinationUserIsTableMember) return TypedResults.NotFound("destination user not found");

                if (!await db.Chips.AnyAsync(c => c.Id == request.ChipId))
                    return TypedResults.NotFound("chip not found");

                var transaction = ChipTransactionBuilder
                    .Create(tableId)
                    .AddChip(request.ChipId, request.ChipCount)
                    .ToUser(request.DestinationUserId)
                    .FromShop()
                    .Build();

                await db.ChipTransactions.AddAsync(transaction);
                await db.SaveChangesAsync();
                return TypedResults.Ok();
            })
            .WithSummary("Create a buy-in transaction")
            .WithDescription("""
                             Creates a buy-in transaction that transfers chips from the table shop
                             to a user in the table players.

                             Only the table manager can perform a buy-in.
                             The destination user must currently be a member of the table.
                             The specified chip must exist.
                             """)
            .Validate<BuyInRequest>()
            .RequireAuthorization();
    }
}