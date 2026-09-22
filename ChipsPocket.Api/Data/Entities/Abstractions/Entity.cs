
namespace ChipsPocket.Api.Data.Entities.Abstractions;

public abstract class Entity : IEntity<Guid>
{
    public Guid Id { get; set; }
}