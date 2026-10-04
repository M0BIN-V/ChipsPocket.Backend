using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface ISeatRepository
{
    public Task ReleaseSeatAsync(Guid tableId, string userId);
    Task<List<Seat>> GetClaimedSeatsAsync(Guid tableId);
    Task<Seat?> GetSeatAsync(Guid tableId, Guid seatId);
}