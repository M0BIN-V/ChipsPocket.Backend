using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Contracts;

public interface IHandActionRepository
{
    void Add(HandAction action);
}