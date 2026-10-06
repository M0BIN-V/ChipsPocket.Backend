using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Infra.Persistence.Repositories;

public class CompletedHandsRepository(AppDbContext db) : ICompletedHandRepository
{
    public Task<CompletedHand?> GetLastHandAsync(Guid tableId)
    {
        return db.CompletedHands.Where(h => h.TableId == tableId)
            .OrderByDescending(h => h.CreatedAtUtc)
            .FirstOrDefaultAsync();
    }

    public Task<CompletedHand?> GetAsync(Guid handId)
    {
        return db.CompletedHands.SingleOrDefaultAsync(h => h.Id == handId);
    }
    

    public void AddHand(CompletedHand completedHand)
    {
        db.CompletedHands.Add(completedHand);
    }
}