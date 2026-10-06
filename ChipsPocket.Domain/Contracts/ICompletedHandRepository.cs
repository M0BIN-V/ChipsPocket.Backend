using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface ICompletedHandRepository
{
    public Task<CompletedHand?> GetLastHandAsync(Guid tableId);
    Task<CompletedHand?> GetAsync(Guid handId);
    public void AddHand(CompletedHand completedHand);
}