using ChipsPocket.Domain.Entities.Abstractions;

namespace ChipsPocket.Domain.Entities;

public class Transaction : Entity
{
    public Guid? TableId { get; init; }
    public Table? Table { get; private set; }

    public string? FromUserId { get; private set; }
    public User? FromUser { get; private set; }

    public string? ToUserId { get; private set; }
    public User? ToUser { get; private set; }

    public Guid? FromPotId { get; private set; }
    public Pot? FromPot { get; private set; }

    public Guid? ToPotId { get; private set; }
    public Pot? ToPot { get; private set; }

    public bool FromShop { get; private set; }
    public bool ToShop { get; private set; }

    public int Value { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; set; }

    internal void SetFromUser(string userId)
    {
        FromUserId = userId;
    }

    internal void SetFromShop(bool fromShop)
    {
        FromShop = fromShop;
    }

    internal void SetFromPot(Guid potId)
    {
        FromPotId = potId;
    }

    internal void SetToUser(string userId)
    {
        ToUserId = userId;
    }

    internal void SetToPot(Guid potId)
    {
        ToPotId = potId;
    }

    internal void SetToShop(bool tooShop)
    {
        ToShop = tooShop;
    }

    internal bool HasSource()
    {
        return FromUserId is not null || FromPotId is not null || FromShop;
    }

    internal bool HasDestination()
    {
        return ToUserId is not null || ToPotId is not null || ToShop;
    }

    internal void SetValue(int value)
    {
        Value = value;
    }
}