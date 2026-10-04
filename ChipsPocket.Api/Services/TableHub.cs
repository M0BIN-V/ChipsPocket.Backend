using ChipsPocket.Api.Extensions;
using ChipsPocket.Api.Realtime;
using ChipsPocket.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChipsPocket.Api.Services;

[Authorize]
[RealtimeDescription("Provides realtime communication for poker tables.")]
public sealed class TableHub(IMembersRepository membersRepository) : Hub
{
    [RealtimeDescription("Should called when user wants to receive table notifications")]
    public async Task JoinTable(Guid tableId)
    {
        var userId = Context.User!.GetUserId();

        if (await membersRepository.IsMemberOfTableAsync(tableId, userId))
            throw new HubException("You cannot access this table.");

        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(tableId));
    }

    public static string GetGroupName(Guid tableId)
    {
        return $"table:{tableId}";
    }
}