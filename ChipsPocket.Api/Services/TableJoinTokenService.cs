using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;

namespace ChipsPocket.Api.Services;

public sealed class TableJoinTokenService(IMemoryCache cache) : ITableJoinTokenService
{
    private const int TokenLength = 8;

    private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(5);

    public string Create(Guid tableId)
    {
        var token = GenerateToken();

        cache.Set(GetCacheKey(token), tableId, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TokenLifetime
        });

        return token;
    }

    public bool TryGetTableId(string token, out Guid tableId)
    {
        return cache.TryGetValue(GetCacheKey(token), out tableId);
    }

    public void Revoke(string token)
    {
        cache.Remove(GetCacheKey(token));
    }

    private static string GetCacheKey(string token)
    {
        return $"table-join:{token}";
    }

    private static string GenerateToken()
    {
        Span<byte> randomBytes = stackalloc byte[TokenLength];

        RandomNumberGenerator.Fill(randomBytes);

        Span<char> token = stackalloc char[TokenLength];

        for (var i = 0; i < TokenLength; i++) token[i] = Alphabet[randomBytes[i] % Alphabet.Length];

        return new string(token);
    }
}