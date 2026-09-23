namespace ChipsPocket.Api.Services;

public sealed class UserStackService(AppDbContext db) : IUserStackService
{
    public async Task<UserStackResponse?> GetAsync(Guid tableId, string userId,
        CancellationToken cancellationToken = default)
    {
        var tableExists = await db.Tables
            .AnyAsync(x => x.Id == tableId, cancellationToken);

        if (!tableExists)
            return null;

        var transactions = await db.ChipTransactions
            .Where(t =>
                t.TableId == tableId &&
                (t.FromUserId == userId ||
                 t.ToUserId == userId))
            .Include(t => t.Chips)
            .ThenInclude(tc => tc.Chip)
            .ToListAsync(cancellationToken);

        var chips = transactions
            .SelectMany(t => t.Chips.Select(tc => new
            {
                tc.ChipId,
                tc.Chip.Name,
                tc.Chip.Picture,
                tc.Chip.Value,

                Count = t.ToUserId == userId
                    ? tc.ChipCount
                    : -tc.ChipCount
            }))
            .GroupBy(x => new
            {
                x.ChipId,
                x.Name,
                x.Picture,
                x.Value
            })
            .Select(g => new UserStackChipResponse(
                g.Key.ChipId,
                g.Key.Name,
                g.Key.Picture,
                g.Key.Value,
                g.Sum(x => x.Count)))
            .Where(x => x.Count > 0)
            .OrderBy(x => x.Value)
            .ToList();

        var totalValue = chips.Sum(x => x.Value * x.Count);

        return new UserStackResponse(
            userId,
            totalValue,
            chips);
    }
}