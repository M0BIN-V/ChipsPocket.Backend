using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Infra.Persistence.Repositories;

public class SeatRepository(AppDbContext db) : ISeatRepository
{
    public Task ReleaseSeatAsync(Guid tableId, string userId)
    {
        return db.Seats.Where(s => s.TableId == tableId && s.UserId == userId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.UserId, _ => null));
    }


    public Task<List<Seat>> GetClaimedSeatsAsync(Guid tableId)
    {
        return db.Seats
            .Where(s => s.TableId == tableId && s.UserId != null)
            .OrderBy(s => s.Order)
            .ToListAsync();
    }

    public Task<Seat?> GetSeatAsync(Guid tableId, Guid seatId)
    {
        return db.Seats
            .Where(s => s.Id == seatId && s.TableId == tableId)
            .OrderBy(s => s.Order)
            .SingleOrDefaultAsync();
    }
}