using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Infra.Persistence.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<List<User>> GetUsersAsync(List<string> userIds)
    {
        return db.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
    }
}