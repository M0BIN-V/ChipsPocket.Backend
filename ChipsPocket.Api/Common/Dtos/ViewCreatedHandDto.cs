namespace ChipsPocket.Api.Common.Dtos;

public record ViewCreatedHandDto(
    Guid HandId,
    Guid DealerSeatId,
    Guid BigBlindSeatId,
    Guid SmallBlindSeatId);