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
}