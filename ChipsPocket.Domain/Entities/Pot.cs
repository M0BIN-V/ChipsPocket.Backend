using ChipsPocket.Domain.Entities.Abstractions;

namespace ChipsPocket.Domain.Entities;

public class Pot : Entity
{
    public Guid HandId { get; set; }
    public Hand Hand { get; private set; } = null!;
}