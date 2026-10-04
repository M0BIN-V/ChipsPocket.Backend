using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;
using ChipsPocket.Domain.Services.UserStack;

namespace ChipsPocket.Domain.Services.HandManager;

public class HandManagerService(
    ITransactionRepository transactionRepository,
    IUserStackService userStackService,
    IHandRepository handRepository,
    ISeatRepository seatRepository,
    ITableRepository tableRepository) : IHandManagerService
{
    public async Task<(bool, string)> TableIsValidToStartHandAsync(Guid tableId)
    {
        //validate active hand
        if (await handRepository.TableHasActiveHandAsync(tableId)) return (false, "table has active hand");

        //validate claimed seat count
        var claimedSeats = await seatRepository.GetClaimedSeatsAsync(tableId);
        if (claimedSeats.Count < 2) return (false, "at least 2 players are required");

        //validate players balance
        var table = await tableRepository.GetTableAsync(tableId);

        if (table is null) throw new NullReferenceException("table not found");
        foreach (var claimedSeat in claimedSeats)
        {
            var requiredAmount = table.BigBlindAmount;

            var userBalance = await userStackService.GetBalanceAsync(tableId, claimedSeat.UserId!);

            if (userBalance < requiredAmount)
                return (false,
                    $"User {claimedSeat.UserId} does not have enough balance to cover the required amount of {requiredAmount}.");
        }

        return (true, string.Empty);
    }

    public async Task<Hand> SetupHandAsync(Guid tableId)
    {
        var claimedSeat = await seatRepository.GetClaimedSeatsAsync(tableId);

        var dealerSeat = SetupDealer(claimedSeat, await GetLastHandDealerSeatOrder(tableId));
        var smallBlindSeat = SetupSmallBlind(claimedSeat, dealerSeat);
        var bigBlindSeat = SetupBigBlind(claimedSeat, smallBlindSeat);

        var hand = new Hand
        {
            TableId = tableId,
            CreatedAtUtc = DateTime.UtcNow,
            CurrentStreet = Street.PreFlop,
            DealerSeatId = dealerSeat.Id,
            SmallBlindSeatId = smallBlindSeat.Id,
            BigBlindSeatId = bigBlindSeat.Id
        };

        return hand;
    }

    private async Task PostBlinds(Hand hand, Guid potId, int smallBlindAmount)
    {
        var smallBlindSeat = await seatRepository.GetSeatAsync(hand.TableId, hand.SmallBlindSeatId);

        var smallBlindTransaction = TransactionBuilder.Create(hand.TableId)
            .FromUser(smallBlindSeat!.UserId!)
            .ToPot(potId)
            .WithAmount(smallBlindAmount)
            .Build();

        var bigBlindSeat = await seatRepository.GetSeatAsync(hand.TableId, hand.BigBlindSeatId);
        var bigBlindTransaction = TransactionBuilder.Create(hand.TableId)
            .FromUser(bigBlindSeat!.UserId!)
            .ToPot(potId)
            .WithAmount(smallBlindAmount * 2)
            .Build();

        transactionRepository.Add(smallBlindTransaction);
        transactionRepository.Add(bigBlindTransaction);
    }

    private async Task<int?> GetLastHandDealerSeatOrder(Guid tableId)
    {
        var dealerSeatId = (await handRepository.GetLastHandAsync(tableId))?.DealerSeatId;

        if (dealerSeatId is null) return null;

        var dealerSeat = await seatRepository.GetSeatAsync(tableId, dealerSeatId.Value);

        return dealerSeat?.Order ?? throw new Exception("dealer seat not found");
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