using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Services;

public class HandRepository(AppDbContext db) : IHandRepository
{
    public Task<Hand?> GetLastHandAsync(Guid tableId)
    {
        return db.Hands.Where(h => h.TableId == tableId)
            .OrderByDescending(h => h.CreatedAtUtc)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> TableHasActiveHandAsync(Guid tableId)
    {
        var lastHand = await GetLastHandAsync(tableId);
        return lastHand is not null && lastHand.CurrentStreet != Street.Finished;
    }
}