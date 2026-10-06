namespace ChipsPocket.Domain.Entities;

public class HandSeat
{
    public int Order { get; set; }
    public required HandPlayer Player { get; set; }
    public bool IsDealer { get; set; }
    public bool IsSmallBlind { get; set; }
    public bool IsBigBlind { get; set; }
    public bool IsFolded { get; set; }
}