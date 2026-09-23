using ChipsPocket.Api.Data.Entities.Abstractions;

namespace ChipsPocket.Api.Data.Entities;

public class Pot : Entity
{
    public Guid TableId { get; set; }
    public Table Table { get; private set; } = null!;
}