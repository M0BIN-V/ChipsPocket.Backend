namespace ChipsPocket.Domain.Services.UserStack;

public interface IUserStackService
{
    Task<int> GetBalanceAsync(
        Guid tableId,
        string userId,
        CancellationToken cancellationToken = default);
}