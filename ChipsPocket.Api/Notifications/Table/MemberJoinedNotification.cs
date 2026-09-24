namespace ChipsPocket.Api.Notifications.Table;

public record MemberJoinedToTableNotification(string UserId, string Username) : ITableNotification;