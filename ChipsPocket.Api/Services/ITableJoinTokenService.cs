namespace ChipsPocket.Api.Services;

public interface ITableJoinTokenService
{
    string Create(Guid tableId);

    bool TryGetTableId(string token, out Guid tableId);

    void Revoke(string token);
}