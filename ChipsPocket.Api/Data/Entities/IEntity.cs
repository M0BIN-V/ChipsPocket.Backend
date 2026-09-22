namespace ChipsPocket.Api.Data.Entities;

public interface IEntity<TId>
{
    public TId Id { get; set; }
}

public abstract class Entity : IEntity<Guid>
{
    public Guid Id { get; set; }
}