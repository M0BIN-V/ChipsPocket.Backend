namespace ChipsPocket.Api.Notifications.Table;

public record PlayerJoinedToTableNotification(string UserId, string Username) : ITableNotification;