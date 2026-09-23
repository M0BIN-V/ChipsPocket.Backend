using ChipsPocket.Api.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChipsPocket.Api.Services;

[Authorize]
[RealtimeDescription("Provides realtime communication for poker tables.")]
public sealed class TableHub : Hub
{
    public static string GetGroupName(Guid tableId)
    {
        return $"table:{tableId}";
    }
}