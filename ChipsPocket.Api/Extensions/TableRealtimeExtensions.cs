using ChipsPocket.Api.Notifications.Table;
using ChipsPocket.Api.Realtime;

namespace ChipsPocket.Api.Extensions;

public static class TableRealtimeExtensions
{
    public static void AddTableRealtime(this RealtimeRegistry realtime)
    {
        realtime
            .RegisterRealtimeEvent<MemberJoinedToTableNotification>(typeof(TableHub),
                "Sent when a player joins a table members")
            .RegisterRealtimeEvent<MemberClaimedSeatNotification>(typeof(TableHub),
                "Sent when a player claims a seat.")
            .RegisterRealtimeEvent<MemberReleasedSeatNotification>(typeof(TableHub),
                "Sent when ap player releases a seat");
    }
}