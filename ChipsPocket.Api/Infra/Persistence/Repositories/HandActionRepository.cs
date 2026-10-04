using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Api.Infra.Persistence.Repositories;

public class HandActionRepository(AppDbContext db) : IHandActionRepository
{
    public void Add(HandAction action)
    {
        db.HandActions.Add(action);
    }
}