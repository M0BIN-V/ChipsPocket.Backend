using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChipsPocket.Api.Services;

public sealed record AuthResponse(string AccessToken, DateTimeOffset ExpiresAt);

public sealed class JwtTokenService(JwtOptions options)
{
    public AuthResponse CreateToken(User user)
    {
        var expiresAt = DateTimeOffset.UtcNow
            .AddMinutes(options.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(options.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            options.Issuer,
            options.Audience,
            claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return new AuthResponse(
            accessToken,
            expiresAt);
    }
}