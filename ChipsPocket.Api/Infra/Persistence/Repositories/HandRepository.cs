using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Infra.Persistence.Repositories;

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

    public Task<Hand?> GetAsync(Guid handId)
    {
        return db.Hands.SingleOrDefaultAsync(h => h.Id == handId);
    }

    public void AddHand(Hand hand)
    {
        db.Hands.Add(hand);
    }
}