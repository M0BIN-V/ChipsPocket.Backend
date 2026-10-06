using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface IMembersRepository
{
    public Task<bool> IsMemberOfTableAsync(Guid tableId, string userId);
    public Task<List<TableMember>> GetTableMembersAsync(Guid tableId);
    public Task<TableMember?> GetTableMemberAsync(Guid tableId , string userId);

    Task<List<Guid>> GetTableIdsAsync(string userId);
    public void AddMember(Guid tableId, string userId);
    Task RemoveAsync(Guid tableId, string userId);
}