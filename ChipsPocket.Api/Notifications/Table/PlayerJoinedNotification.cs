namespace ChipsPocket.Api.Notifications.Table;

public record PlayerJoinedToLobbyNotification(string UserId, string Username) : ITableNotification;