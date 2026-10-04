using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;
using ChipsPocket.Domain.Services.HandActionManager;
using ChipsPocket.Domain.Services.UserStack;

namespace ChipsPocket.Domain.Services.HandManager;

public class HandManagerService(
    IHandActionManager actionManager,
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
            var userBalance = await userStackService.GetBalanceAsync(tableId, claimedSeat.UserId!);

            if (userBalance < 0) return (false, $"User {claimedSeat.UserId} does not have enough balance.");
        }

        return (true, string.Empty);
    }

    public async Task<Hand> SetupHandAsync(Guid tableId)
    {
        var claimedSeats = await seatRepository.GetClaimedSeatsAsync(tableId);

        var dealerSeat = SetupDealer(claimedSeats, await GetLastHandDealerSeatOrder(tableId));
        var smallBlindSeat = SetupSmallBlind(claimedSeats, dealerSeat);
        var bigBlindSeat = SetupBigBlind(claimedSeats, smallBlindSeat);

        var table = await tableRepository.GetTableAsync(tableId);

        var smallBlindAmount = table?.SmallBlindAmount ?? throw new Exception("table not found");
        var bigBlindAmount = smallBlindAmount * 2;

        var hand = new Hand
        {
            TableId = tableId,
            DealerSeatId = dealerSeat.Id,
            SmallBlindSeatId = smallBlindSeat.Id,
            BigBlindSeatId = bigBlindSeat.Id,
            CurrentStreet = Street.PreFlop,
            MinimumRaise = bigBlindAmount * 2,
            NextActorSeatId = NextClaimedSeat(claimedSeats, bigBlindSeat.Order).Id
        };

        await actionManager.PostSmallBlindAsync(hand.Id, smallBlindSeat.Id, smallBlindAmount);
        await actionManager.PostBigBlindAsync(hand.Id, bigBlindSeat.Id, bigBlindAmount);

        return hand;
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