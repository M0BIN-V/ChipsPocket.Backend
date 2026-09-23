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

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs"))
                            context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddSingleton<JwtTokenService>();

        builder.Services.AddAuthorization();
    }
}