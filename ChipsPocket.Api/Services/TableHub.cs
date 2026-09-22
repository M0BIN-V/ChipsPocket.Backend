using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChipsPocket.Api.Services;

[Authorize]
public sealed class TableHub : Hub
{
    public async Task JoinTable(Guid tableId)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            GetGroupName(tableId));
    }

    public async Task LeaveTable(Guid tableId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            GetGroupName(tableId));
    }

    public static string GetGroupName(Guid tableId)
    {
        return $"table:{tableId}";
    }
}