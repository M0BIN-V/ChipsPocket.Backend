namespace ChipsPocket.Domain.Services.Shop;

public interface IShopService
{
    public void BuyChips(Guid tableId, string userId, int amount);

    public void SellChips(Guid tableId, string userId, int amount);
}