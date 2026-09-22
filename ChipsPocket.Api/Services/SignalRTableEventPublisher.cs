using Microsoft.AspNetCore.SignalR;

namespace ChipsPocket.Api.Services;

public sealed class SignalRTableEventPublisher(
    IHubContext<TableHub> hubContext)
    : ITableEventPublisher
{
    public Task PublishAsync(
        Guid tableId,
        ITableEvent @event,
        CancellationToken cancellationToken = default)
    {
        return hubContext.Clients
            .Group(TableHub.GetGroupName(tableId))
            .SendAsync(
                @event.GetType().Name,
                @event,
                cancellationToken);
    }
}