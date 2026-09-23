namespace ChipsPocket.Api.Notifications.Table;

public record PlayerClaimedSeatNotification(Guid SeatId, string UserId, string Username) : ITableNotification;