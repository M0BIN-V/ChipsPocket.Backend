namespace ChipsPocket.Domain.Services.Shop;

public interface IShopService
{
    public void BuyChipsAsync(Guid tableId, string userId, int amount);

    public void SellChipsAsync(Guid tableId, string userId, int amount);
}