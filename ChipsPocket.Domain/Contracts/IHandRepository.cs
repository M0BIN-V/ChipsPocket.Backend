using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface IHandRepository
{
    public Task<Hand?> GetLastHandAsync(Guid tableId);
    public Task<bool> TableHasActiveHandAsync(Guid tableId);
    Task<Hand?> GetAsync(Guid handId);
    public void AddHand(Hand hand);
}