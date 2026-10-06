using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Services.HandManager;

public interface IHandManagerService
{
    public Task<(bool, string)> TableIsValidToStartHandAsync(Guid tableId);
    public Task<ActiveHand> SetupHandAsync(Guid tableId);
}