namespace ChipsPocket.Api.Services;

public interface ITableNotification;

public interface ITableNotificationPublisher
{
    Task PublishAsync(Guid tableId, ITableNotification notification, CancellationToken cancellationToken = default);
}