namespace ChipsPocket.Api.Services;

public interface IMemberService
{
    public Task<bool> IsMemberOfTableAsync(Guid tableId, string userId);
    public Task<List<TableMember>> GetTableMembersAsync(Guid tableId);
    Task<List<Guid>> GetTableIdsAsync(string userId);
    public void AddMember(Guid tableId, string userId);
    Task RemoveAsync(Guid tableId, string userId);
}

public class MemberService(AppDbContext db) : IMemberService
{
    public Task<bool> IsMemberOfTableAsync(Guid tableId, string userId)
    {
        return db.TableMembers.AnyAsync(m => m.TableId == tableId && m.UserId == userId);
    }

    public Task<List<TableMember>> GetTableMembersAsync(Guid tableId)
    {
        return db.TableMembers.Where(m => m.TableId == tableId).ToListAsync();
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