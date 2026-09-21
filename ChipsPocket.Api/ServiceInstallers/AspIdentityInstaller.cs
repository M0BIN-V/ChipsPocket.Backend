using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class AspIdentityInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        
        builder.Services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = false;

                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
}