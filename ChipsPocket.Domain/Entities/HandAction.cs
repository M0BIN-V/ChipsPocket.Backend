namespace ChipsPocket.Domain.Entities;

public class HandAction
{
    public required DateTimeOffset ActionTimeUtc { get; set; }
    public required Street Street { get; set; }
    public required int ActorSeatOrder { get; set; }
    public required HandActionType Type { get; set; }
    public required int? Amount { get; set; }
    public required bool IsAllIn { get; set; }
}