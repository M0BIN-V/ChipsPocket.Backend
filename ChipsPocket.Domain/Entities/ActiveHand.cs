namespace ChipsPocket.Domain.Entities;

public class ActiveHand
{
    public required Guid TableId { get; init; }
    public required Guid HandId { get; init; }
    public required List<HandSeat> Seats { get; set; } = [];
    public required int PotValue { get; set; }
    public required int BigBlindAmount { get; set; }
    public required int SmallBlindAmount { get; set; }
    public required int MinimumRaiseAmount { get; set; }
    public required Street CurrentStreet { get; set; }
    public required List<HandAction> Actions { get; set; } = [];

    // public int NextSeatToActionOrder => GetNextSeatToActionOrder();
    //
    // private int GetNextSeatToActionOrder()
    // {
    //     var currentStreetActions = GetCurrentStreetActions(Actions, CurrentStreet);
    //
    //     var maxPlayerActionCount = GetMaxPlayerActionCount(currentStreetActions);
    //
    //     var activeSeats = Seats.Where(s =>
    //         s is
    //         {
    //             IsFolded: false,
    //             Player.Stack: > 0
    //         });
    //
    //     return 0;
    // }

    private static int GetMaxPlayerActionCount(List<HandAction> actions)
    {
        var previousActorSeatOrder = actions.OrderBy(a => a.ActionTimeUtc).LastOrDefault()?.ActorSeatOrder;
        if (previousActorSeatOrder is null) return 0;
        return actions.Count(a => a.ActorSeatOrder == previousActorSeatOrder);
    }

    private static IEnumerable<HandAction> GetCurrentStreetActions(IEnumerable<HandAction> actions,
        Street currentStreet)
    {
        return actions.Where(a =>
            a.Street == currentStreet &&
            a.Type
                is not HandActionType.PostBigBlind
                and not HandActionType.PostSmallBlind);
    }
}