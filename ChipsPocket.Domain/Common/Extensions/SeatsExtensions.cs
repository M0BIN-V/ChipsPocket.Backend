namespace ChipsPocket.Domain.Common.Extensions;

public static class SeatsExtensions
{
    extension(List<Seat> claimedSeats)
    {
        public Seat GetDealer(int? lastDealerOrder)
        {
            return lastDealerOrder is null ? claimedSeats[0] : claimedSeats.GetNextClaimedSeat(lastDealerOrder.Value);
        }

        public Seat GetSmallBlind(Seat dealer)
        {
            return claimedSeats.Count == 2
                ? dealer
                : claimedSeats.GetNextClaimedSeat(dealer.Order);
        }

        public Seat GetBigBlind(Seat smallBlind)
        {
            return claimedSeats.Count == 2
                ? smallBlind
                : claimedSeats.GetNextClaimedSeat(smallBlind.Order);
        }

        public Seat GetNextClaimedSeat(int currentOrder)
        {
            return claimedSeats
                .Where(s => s.Order > currentOrder)
                .Concat(claimedSeats.Where(s => s.Order <= currentOrder))
                .First();
        }
    }
}