namespace ChipsPocket.Domain.Entities.Abstractions;

public interface IEntity<TId>
{
    public TId Id { get; set; }
}