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