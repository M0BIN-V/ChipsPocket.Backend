namespace ChipsPocket.Domain.Entities.Abstractions;

public abstract class Entity : IEntity<Guid>
{
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public Guid Id { get; set; } = Guid.CreateVersion7();
}