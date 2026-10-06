using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Services.HandManager;

public class HandManagerService(
    IMembersRepository membersRepo,
    IActiveHandRepository activeHandRepo,
    ICompletedHandRepository completedHandRepo,
    ISeatRepository seatRepository,
    ITableRepository tableRepository) : IHandManagerService
{
    public async Task<(bool, string)> TableIsValidToStartHandAsync(Guid tableId)
    {
        //validate active hand
        if (activeHandRepo.GetHand(tableId) is not null) return (false, "table has active hand");

        //validate claimed seat count
        var claimedSeats = await seatRepository.GetClaimedSeatsAsync(tableId);
        if (claimedSeats.Count < 2) return (false, "at least 2 players are required");

        //validate players balance
        var table = await tableRepository.GetTableAsync(tableId);

        if (table is null) throw new NullReferenceException("table not found");
        foreach (var claimedSeat in claimedSeats)
        {
            var member = await membersRepo.GetTableMemberAsync(tableId, claimedSeat.UserId!);

            if (member!.Stack <= 0) return (false, $"User {claimedSeat.UserId} does not have enough balance.");
        }

        return (true, string.Empty);
    }

    public async Task<ActiveHand> SetupHandAsync(Guid tableId)
    {
        var claimedSeats = await seatRepository.GetClaimedSeatsAsync(tableId);

        var dealerSeat = SetupDealer(claimedSeats, await GetLastHandDealerSeatOrder(tableId));
        var smallBlindSeat = SetupSmallBlind(claimedSeats, dealerSeat);
        var bigBlindSeat = SetupBigBlind(claimedSeats, smallBlindSeat);

        var table = await tableRepository.GetTableAsync(tableId);

        if (table is null) throw new NullReferenceException("table not found");

        var usersStacks = (await membersRepo.GetTableMembersAsync(tableId))
            .ToDictionary(k => k.UserId, v => v.Stack);

        var activeHand = new ActiveHand
        {
            TableId = tableId,
            HandId = Guid.CreateVersion7(),
            Seats = claimedSeats.Select(s => new HandSeat
                {
                    Order = s.Order,
                    Player = new HandPlayer
                    {
                        UserId = s.UserId!,
                        Username = s.User!.UserName!,
                        Stack = usersStacks[s.UserId!]
                    },
                    IsDealer = dealerSeat.Id == s.Id,
                    IsSmallBlind = smallBlindSeat.Id == s.Id,
                    IsBigBlind = bigBlindSeat.Id == s.Id,
                    IsFolded = false
                })
                .ToList(),
            PotValue = 0,
            BigBlindAmount = table.BigBlindAmount,
            SmallBlindAmount = table.SmallBlindAmount,
            MinimumRaiseAmount = table.BigBlindAmount * 2,
            CurrentStreet = Street.PreFlop,
            Actions = []
        };

        return activeHand;
    }

    private async Task<int?> GetLastHandDealerSeatOrder(Guid tableId)
    {
        var dealerSeatId = (await completedHandRepo.GetLastHandAsync(tableId))?.DealerSeatId;

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