namespace ChipsPocket.Api.Notifications.Table;

public record PlayerReleasedSeatNotification(string UserId , Guid SeatId): ITableNotification;