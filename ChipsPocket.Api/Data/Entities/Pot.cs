using ChipsPocket.Api.Data.Entities.Abstractions;

namespace ChipsPocket.Api.Data.Entities;

public class Pot : Entity
{
    public Guid HandId { get; set; }
    public Hand Hand { get; private set; } = null!;
}