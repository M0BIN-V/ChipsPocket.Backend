namespace ChipsPocket.Api.Notifications.Table;

public record MemberClaimedSeatNotification(Guid SeatId, string UserId, string Username) : ITableNotification;