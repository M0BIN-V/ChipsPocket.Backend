using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Infra.Persistence.Repositories;

public class TableRepository(AppDbContext db) : ITableRepository
{
    public Task<Table?> GetTableAsync(Guid tableId)
    {
        return db.Tables.SingleOrDefaultAsync(t => t.Id == tableId);
    }

    public Task<string?> GetManagerIdAsync(Guid tableId)
    {
        return db.Tables
            .Where(t => t.Id == tableId)
            .Select(t => t.ManagerId)
            .SingleOrDefaultAsync();
    }

    public Task<List<Table>> GetTablesAsync(List<Guid> userTableIds)
    {
        return db.Tables
            .Where(t => userTableIds.Contains(t.Id))
            .ToListAsync();
    }
}