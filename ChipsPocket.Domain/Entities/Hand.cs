using ChipsPocket.Domain.Entities.Abstractions;

namespace ChipsPocket.Domain.Entities;

public class Hand : Entity
{
    public Guid TableId { get; set; }

    public Guid WaitingForActionId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public Street CurrentStreet { get; set; }

    public Guid BigBlindSeatId { get; set; }
    public Guid SmallBlindSeatId { get; set; }
    public Guid DealerSeatId { get; set; }
}