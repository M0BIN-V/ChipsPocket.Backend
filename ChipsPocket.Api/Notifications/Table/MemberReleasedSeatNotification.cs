namespace ChipsPocket.Api.Notifications.Table;

public record MemberReleasedSeatNotification(string UserId , Guid SeatId): ITableNotification;

public record HandStartedNotification(Guid HandId) : ITableNotification;