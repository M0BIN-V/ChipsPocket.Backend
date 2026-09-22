namespace ChipsPocket.Api.Data.Entities.Abstractions;

public interface IEntity<TId>
{
    public TId Id { get; set; }
}