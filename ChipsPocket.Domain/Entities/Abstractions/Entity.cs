namespace ChipsPocket.Domain.Entities.Abstractions;

public abstract class Entity : IEntity<Guid>
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Guid Id { get; set; } = Guid.CreateVersion7();
}