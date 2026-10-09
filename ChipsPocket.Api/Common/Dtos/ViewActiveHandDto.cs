using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Common.Dtos;

public class ViewActiveHandDto
{
    public required Guid TableId { get; init; }
    public required Guid HandId { get; init; }
    public required List<ViewHandSeatDto> Seats { get; set; } = [];
    public required int PotValue { get; set; }
    public required int BigBlindAmount { get; set; }
    public required int SmallBlindAmount { get; set; }
    public required int MinimumRaiseAmount { get; set; }
    public required Street CurrentStreet { get; set; }
    public required List<ViewHandActionDto> Actions { get; set; } = [];
}

public class ViewHandSeatDto
{
    public int Order { get; set; }
    public required ViewHandPlayerDto Player { get; set; }
    public bool IsDealer { get; set; }
    public bool IsSmallBlind { get; set; }
    public bool IsBigBlind { get; set; }
    public bool IsFolded { get; set; }
}

public class ViewHandPlayerDto
{
    public required string UserId { get; set; }
    public required string Username { get; set; }
    public required int Stack { get; set; }
}

public class ViewHandActionDto
{
    public Street Street { get; set; }
    public int ActorSeatOrder { get; set; }
    public HandActionType Type { get; set; }
    public int? Amount { get; set; }
    public bool IsAllIn { get; set; }
}