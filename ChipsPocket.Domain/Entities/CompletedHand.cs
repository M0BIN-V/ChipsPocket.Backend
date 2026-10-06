using ChipsPocket.Domain.Entities.Abstractions;

namespace ChipsPocket.Domain.Entities;

public class CompletedHand : Entity
{
    public required Guid TableId { get; init; }
    public required Guid BigBlindSeatId { get; init; }
    public required Guid SmallBlindSeatId { get; init; }
    public required Guid DealerSeatId { get; init; }
}