using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Entities;

namespace ChipsPocket.Domain.Services.Shop;

public class ShopService(ITransactionRepository transactionRepo) : IShopService
{
    public void BuyChips(Guid tableId, string userId, int amount)
    {
        var transaction = TransactionBuilder
            .Create(tableId)
            .FromShop()
            .ToUser(userId)
            .WithAmount(amount)
            .Build();

        transactionRepo.Add(transaction);
    }

    public void SellChips(Guid tableId, string userId, int amount)
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