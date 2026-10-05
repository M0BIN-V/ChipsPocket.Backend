using ChipsPocket.Api.Common.Dtos;

namespace ChipsPocket.Api.Notifications.Table;


public record MemberClaimedSeatNotification(Guid SeatId, string UserId, string Username) : ITableNotification;

public record MemberJoinedToTableNotification(string UserId, string Username) : ITableNotification;

public record MemberReleasedSeatNotification(string UserId, Guid SeatId) : ITableNotification;

public record HandStartedNotification(ViewCreatedHandDto Hand) : ITableNotification;