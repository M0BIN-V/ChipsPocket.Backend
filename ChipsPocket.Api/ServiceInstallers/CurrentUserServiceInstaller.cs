using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class CurrentUserServiceInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUser, CurrentUser>();
    }
}