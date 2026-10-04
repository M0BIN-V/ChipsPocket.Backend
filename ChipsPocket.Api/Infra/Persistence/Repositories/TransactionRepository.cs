using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Infra.Persistence.Repositories;

public class TransactionRepository(AppDbContext db) : ITransactionRepository
{
    public void Add(Transaction transaction)
    {
        db.Transactions.Add(transaction);
    }

    public Task<List<Transaction>> GetUserTransactionsAsync(Guid tableId, string userId)
    {
        return db.Transactions
            .Where(t => t.TableId == tableId && (t.FromUserId == userId || t.ToUserId == userId))
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToListAsync();
    }
}