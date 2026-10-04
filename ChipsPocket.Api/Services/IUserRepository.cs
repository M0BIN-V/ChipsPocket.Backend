using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Services;

public interface IUserRepository
{
    public Task<List<User>> GetUsersAsync(List<string> userIds);
}

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<List<User>> GetUsersAsync(List<string> userIds)
    {
        return db.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
    }
}