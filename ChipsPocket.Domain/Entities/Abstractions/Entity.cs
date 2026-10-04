namespace ChipsPocket.Domain.Entities.Abstractions;

public abstract class Entity : IEntity<Guid>
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
}