namespace ChipsPocket.Api.Services;

public interface ITableEvent;

public sealed record PlayerJoinedTableEvent(
    Guid UserId,
    string DisplayName) : ITableEvent;

public interface ITableEventPublisher
{
    Task PublishAsync(
        Guid tableId,
        ITableEvent @event,
        CancellationToken cancellationToken = default);
}