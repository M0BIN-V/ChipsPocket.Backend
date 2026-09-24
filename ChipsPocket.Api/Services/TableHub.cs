using System.ComponentModel;
using ChipsPocket.Api.Extensions;
using ChipsPocket.Api.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChipsPocket.Api.Services;

[Authorize]
[RealtimeDescription("Provides realtime communication for poker tables.")]
public sealed class TableHub(AppDbContext db) : Hub
{
    [RealtimeDescription("Should called when user wants to receive table notifications")]
    public async Task JoinTable(Guid tableId)
    {
        var userId = Context.User!.GetUserId();

        var hasAccess = await db.Tables
            .AnyAsync(t => t.Id == tableId && t.Members
                .Any(p => p.UserId == userId));

        if (!hasAccess) throw new HubException("You cannot access this table.");

        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(tableId));
    }

    public static string GetGroupName(Guid tableId)
    {
        return $"table:{tableId}";
    }
}