using ChipsPocket.Api.Endpoints.Table.UserStack;

namespace ChipsPocket.Api.Services;

public interface IUserStackService
{
    Task<UserStackResponse> GetAsync(
        Guid tableId,
        string userId,
        CancellationToken cancellationToken = default);
}


public sealed record UserStackChipResponse(
    Guid ChipId,
    string Name,
    string Picture,
    int Value,
    int Count);

public sealed record UserStackResponse(
    string UserId,
    int TotalValue,
    IReadOnlyList<UserStackChipResponse> Chips);