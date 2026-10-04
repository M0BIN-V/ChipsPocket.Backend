using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface ITableRepository
{
    public Task<Table?> GetTableAsync(Guid tableId);
    public Task<string?> GetManagerIdAsync(Guid tableId);
    public Task<List<Table>> GetTablesAsync(List<Guid> userTableIds);
}