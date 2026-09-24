namespace ChipsPocket.Api.Endpoints.Table.Hands.StartNewHand;

public class StartHandEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("", async Task<Results<
            NotFound<string>,
            ForbidHttpResult,
            BadRequest<string>,
            Ok<string>>> (
            [FromRoute] Guid tableId,
            [FromServices] IUserStackService userStackService,
            [FromServices] ICurrentUser currentUser,
            [FromServices] AppDbContext db) =>
        {
            var table = await db.Tables.Where(t => t.Id == tableId)
                .Select(t => new
                {
                    t.SmallBlindAmount,
                    t.BigBlindAmount,
                    t.ManagerId,
                    HasActiveHand = t.Hands.Any(h => h.CurrentStreet != Street.Finished),
                    LastHand = t.Hands
                        .OrderByDescending(h => h.CreatedAt)
                        .Select(h => new
                        {
                            DealerOrder = h.DealerSeat.Order
                        })
                        .FirstOrDefault(),
                    PlayersCount = t.Members.Count(),
                    ClaimedSeatCount = t.Seats.Count(s => s.UserId != null)
                })
                .SingleOrDefaultAsync();

            if (table is null) return TypedResults.NotFound("Table not found");
            if (table.ManagerId != currentUser.Id) return TypedResults.Forbid();
            if (table.HasActiveHand) return TypedResults.BadRequest("Table already has an active hand");

            if (table.ClaimedSeatCount != table.PlayersCount)
                return TypedResults.BadRequest(
                    "Not all players in the lobby have claimed a seat. All players must claim a seat before starting a hand.");

            if (table.ClaimedSeatCount < 2)
                return TypedResults.BadRequest("Not enough players to start a hand. At least 2 players are required.");

            var claimedSeats = await db.Seats
                .Where(s => s.TableId == tableId && s.UserId != null)
                .OrderBy(s => s.Order)
                .ToListAsync();

            var dealer = SetupDealer(claimedSeats, table.LastHand?.DealerOrder);
            var smallBlind = SetupSmallBlind(claimedSeats, dealer);
            var bigBlind = SetupBigBlind(claimedSeats, smallBlind);

            foreach (var seat in claimedSeats)
            {
                var requiredAmount = seat == smallBlind
                    ? table.SmallBlindAmount
                    : seat == bigBlind
                        ? table.BigBlindAmount
                        : 1;

                var response = await userStackService.GetAsync(tableId, seat.UserId!);

                if (response.TotalValue < requiredAmount)
                    return TypedResults.BadRequest(
                        $"User {seat.UserId} does not have enough balance to cover the required amount of {requiredAmount}.");
            }

            var hand = new Hand
            {
                TableId = tableId,
                CreatedAt = DateTimeOffset.UtcNow,
                CurrentStreet = Street.Pending,
                DealerSeatId = dealer.Id,
                SmallBlindSeatId = smallBlind.Id,
                BigBlindSeatId = bigBlind.Id
            };

            await db.Hands.AddAsync(hand);
            await db.SaveChangesAsync();

            return TypedResults.Ok("Hand started successfully");
        });
    }

    private static Seat SetupDealer(List<Seat> claimedSeats, int? lastDealerOrder)
    {
        return lastDealerOrder is null ? claimedSeats[0] : NextClaimedSeat(claimedSeats, lastDealerOrder.Value);
    }

    private static Seat SetupSmallBlind(List<Seat> claimedSeats, Seat dealer)
    {
        return claimedSeats.Count == 2
            ? dealer
            : NextClaimedSeat(claimedSeats, dealer.Order);
    }

    private static Seat SetupBigBlind(List<Seat> claimedSeats, Seat smallBlind)
    {
        return claimedSeats.Count == 2
            ? smallBlind
            : NextClaimedSeat(claimedSeats, smallBlind.Order);
    }

    private static Seat NextClaimedSeat(List<Seat> claimedSeats, int currentOrder)
    {
        return claimedSeats
            .Where(s => s.Order > currentOrder)
            .Concat(claimedSeats.Where(s => s.Order <= currentOrder))
            .First();
    }
}