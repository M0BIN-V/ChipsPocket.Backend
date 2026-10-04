using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Domain.Services.UserStack;

public sealed class UserStackService(ITransactionRepository transactionsRepo) : IUserStackService
{
    public async Task<int> GetBalanceAsync(Guid tableId, string userId,
        CancellationToken cancellationToken = default)
    {
        var userTransactionsFromTheTable = await transactionsRepo.GetUserTransactionsAsync(tableId, userId);

        var values = userTransactionsFromTheTable
            .Select(t => t.ToUserId == userId ? t.Value : -t.Value)
            .ToList();

        var total = values.Sum();

        return total;
    }
}