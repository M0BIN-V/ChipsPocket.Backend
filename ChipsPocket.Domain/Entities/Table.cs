using ChipsPocket.Domain.Entities.Abstractions;

namespace ChipsPocket.Domain.Entities;

public class Table : Entity
{
    private Table()
    {
    }

    public ICollection<Hand> Hands { get; private set; } = [];

    public ICollection<Seat> Seats { get; private set; } = [];

    public int BigBlindAmount => SmallBlindAmount * 2;
    public int SmallBlindAmount { get; set; }

    public string CreatedById { get; private set; } = null!;
    public User CreatedBy { get; private set; } = null!;

    public string ManagerId { get; set; } = null!;
    public User Manager { get; private set; } = null!;

    public required string Name { get; set; }

    public static Table Create(string name, string creatorId, int smallBlind)
    {
        var id = Guid.CreateVersion7();

        return new Table
        {
            SmallBlindAmount = smallBlind,
            Id = id,
            CreatedById = creatorId,
            Name = name
        };
    }
}