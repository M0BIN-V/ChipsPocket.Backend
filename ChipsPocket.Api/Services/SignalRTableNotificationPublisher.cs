using Microsoft.AspNetCore.SignalR;

namespace ChipsPocket.Api.Services;

public sealed class SignalRTableNotificationPublisher(IHubContext<TableHub> hubContext) : ITableNotificationPublisher
{
    public Task PublishAsync(Guid tableId, ITableNotification notification, CancellationToken cancellationToken = default)
    {
        return hubContext.Clients
            .Group(TableHub.GetGroupName(tableId))
            .SendAsync(notification.GetType().Name, notification, cancellationToken);
    }
}