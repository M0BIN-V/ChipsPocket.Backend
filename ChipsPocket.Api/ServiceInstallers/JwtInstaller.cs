using DiServiceInstaller;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ChipsPocket.Api.ServiceInstallers;

public class JwtInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        var jwtOptions = builder.Configuration
                             .GetSection(JwtOptions.SectionName)
                             .Get<JwtOptions>()
                         ?? throw new InvalidOperationException("JWT configuration is missing.");

        builder.Services.AddSingleton(jwtOptions);

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddSingleton<JwtTokenService>();

        builder.Services.AddAuthorization();
    }
}