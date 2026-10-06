using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface ITransactionRepository
{
    public void Add(Transaction transaction);
    public Task<List<Transaction>> GetUserTransactionsAsync(Guid tableId, string userId);
    public Task<List<Transaction>> GetPotTransaction(Guid potId);
}