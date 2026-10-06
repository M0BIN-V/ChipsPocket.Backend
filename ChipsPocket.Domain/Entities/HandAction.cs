namespace ChipsPocket.Domain.Entities;

public class HandAction
{
    public Street Street { get; set; }
    public int ActorSeatOrder { get; set; }
    public HandActionType Type { get; set; }
    public int? Amount { get; set; }
    public bool IsAllIn { get; set; }
}