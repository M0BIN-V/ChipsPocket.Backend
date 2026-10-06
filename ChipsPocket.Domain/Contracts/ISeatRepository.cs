using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface ISeatRepository
{
    Task ReleaseSeatAsync(Guid tableId, string userId);
    Task<List<Seat>> GetClaimedSeatsAsync(Guid tableId);
    Task<List<Seat>> GetSeatsAsync(Guid tableId, bool includeUser = true);
    Task<Seat?> GetSeatAsync(Guid tableId, Guid seatId);
}