using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Services.HandActionManager;

public interface IHandActionManager
{
    public Task PostSmallBlindAsync(
        Hand hand,
        int amount);

    public Task PostBigBlindAsync(
        Hand hand,
        int amount);
}