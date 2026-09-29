namespace ChipsPocket.Api.Services;

public interface IUserStackService
{
    Task<int> GetBalanceAsync(
        Guid tableId,
        string userId,
        CancellationToken cancellationToken = default);
}

