using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface IHandRepository
{
    public Task<Hand?> GetLastHandAsync(Guid tableId);
    public Task<bool> TableHasActiveHandAsync(Guid tableId);
}