using ChipsPocket.Domain.Entities.Abstractions;

namespace ChipsPocket.Domain.Entities;

public class HandAction : Entity
{
    public Guid HandId { get; set; }
    public Street Street { get; set; }
    public Guid ActorSeatId { get; set; }
    public HandActionType Type { get; set; }
    public int? Amount { get; set; }
    public bool IsAllIn { get; set; }
}