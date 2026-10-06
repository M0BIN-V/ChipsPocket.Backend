using ChipsPocket.Domain.Contracts;

namespace ChipsPocket.Domain.Services.Pot;

public interface IPotService
{
    public Task<int> GetPotValue(Guid potId);
}

public class PotService(ITransactionRepository transactionRepo) : IPotService
{
    public async Task<int> GetPotValue(Guid potId)
    {
        var potTransactions = await transactionRepo.GetPotTransaction(potId);

        var values = potTransactions
            .Select(t => t.ToPotId == potId ? t.Value : -t.Value)
            .ToList();

        var total = values.Sum();

        return total;
    }
}