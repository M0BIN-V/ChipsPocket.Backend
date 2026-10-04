using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface IUserRepository
{
    public Task<List<User>> GetUsersAsync(List<string> userIds);
}

