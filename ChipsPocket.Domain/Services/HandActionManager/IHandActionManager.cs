using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Services.HandActionManager;

public interface IHandActionManager
{
    void PostSmallBlind(ActiveHand hand);

    void PostBigBlind(ActiveHand hand);
}