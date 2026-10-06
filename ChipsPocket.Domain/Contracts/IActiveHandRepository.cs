using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface IActiveHandRepository
{
    void AddHand(ActiveHand hand);
    ActiveHand? GetHand(Guid tableId);
    void SaveHand(ActiveHand hand);
    bool RemoveHand(Guid tableId);
}