using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Infra.Persistence.Repositories;

public class MembersRepository(AppDbContext db) : IMembersRepository
{
    public Task<bool> IsMemberOfTableAsync(Guid tableId, string userId)
    {
        return db.TableMembers.AnyAsync(m => m.TableId == tableId && m.UserId == userId);
    }

    public Task<List<TableMember>> GetTableMembersAsync(Guid tableId)
    {
        return db.TableMembers.Where(m => m.TableId == tableId).ToListAsync();
    }

    public Task<TableMember?> GetTableMemberAsync(Guid tableId, string userId)
    {
        return db.TableMembers.SingleOrDefaultAsync(m => m.TableId == tableId && m.UserId == userId);
    }

    public Task<List<Guid>> GetTableIdsAsync(string userId)
    {
        return db.TableMembers
            .Where(m => m.UserId == userId)
            .Select(m => m.TableId)
            .ToListAsync();
    }

    public void AddMember(Guid tableId, string userId)
    {
        db.TableMembers.Add(new TableMember
        {
            UserId = userId,
            TableId = tableId
        });
    }

    public Task RemoveAsync(Guid tableId, string userId)
    {
        return db.TableMembers.Where(m => m.TableId == tableId && m.UserId == userId)
            .ExecuteDeleteAsync();
    }
}