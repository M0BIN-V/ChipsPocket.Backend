namespace ChipsPocket.Domain.Services.HandActionManager;

public interface IHandActionManager
{
    public Task PostSmallBlindAsync(
        Guid handId,
        Guid seatId,
        int amount);

    public Task PostBigBlindAsync(
        Guid handId,
        Guid seatId,
        int amount);
}