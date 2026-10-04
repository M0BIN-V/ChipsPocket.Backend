using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Services.Shop;

public class ShopService(ITransactionRepository transactionRepo) : IShopService
{
    public void BuyChipsAsync(Guid tableId, string userId, int amount)
    {
        var transaction = TransactionBuilder
            .Create(tableId)
            .FromShop()
            .ToUser(userId)
            .WithAmount(amount)
            .Build();

        transactionRepo.Add(transaction);
    }

    public void SellChipsAsync(Guid tableId, string userId, int amount)
    {
        var transaction = TransactionBuilder
            .Create(tableId)
            .FromUser(userId)
            .ToShop()
            .WithAmount(amount)
            .Build();

        transactionRepo.Add(transaction);
    }
}