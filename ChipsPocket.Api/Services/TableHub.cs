using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChipsPocket.Api.Services;

[Authorize]
public sealed class TableHub : Hub
{
    public static string GetGroupName(Guid tableId)
    {
        return $"table:{tableId}";
    }
}