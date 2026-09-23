using ChipsPocket.Api.Notifications.Table;
using ChipsPocket.Api.Realtime;

namespace ChipsPocket.Api.Extensions;

public static class TableRealtimeExtensions
{
    public static void AddTableRealtime(this RealtimeRegistry realtime)
    {
        realtime
            .RegisterRealtimeEvent<PlayerJoinedNotification>(typeof(TableHub),
                "Sent when a player joins a table.");

        realtime
            .RegisterRealtimeEvent<PlayerClaimedSeatNotification>(typeof(TableHub),
                "Sent when a player leaves a table.");
    }
}