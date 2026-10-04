using ChipsPocket.Domain.Entities.Abstractions;

namespace ChipsPocket.Domain.Entities;

public class Hand : Entity
{
    public Hand()
    {
        Pot = new Pot();
        PotId = Pot.Id;
    }

    public required Guid TableId { get; init; }
    public Street CurrentStreet { get; set; }
    public required Guid BigBlindSeatId { get; init; }
    public required Guid SmallBlindSeatId { get; init; }
    public required Guid DealerSeatId { get; init; }

    public int MinimumRaise { get; set; }

    public Guid PotId { get; private set; }
    public Pot Pot { get; }
    public Guid NextActorSeatId { get; set; }
}