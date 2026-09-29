namespace ChipsPocket.Api.Services;

public sealed class UserStackService(AppDbContext db) : IUserStackService
{
    public async Task<int> GetBalanceAsync(Guid tableId, string userId,
        CancellationToken cancellationToken = default)
    {
        var tableExists = await db.Tables.AnyAsync(x => x.Id == tableId, cancellationToken);

        if (!tableExists) throw new InvalidOperationException($"Table with ID {tableId} does not exist.");

        var userTransactionsFromTheTable = await db.ChipTransactions
            .Where(t => t.TableId == tableId && (t.FromUserId == userId || t.ToUserId == userId))
            .ToListAsync(cancellationToken);

        var values = userTransactionsFromTheTable
            .Select(t => t.ToUserId == userId ? t.Value : -t.Value)
            .ToList();

        var total = values.Sum();

        return total;
    }
}