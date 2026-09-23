using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class UserStackServiceInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserStackService, UserStackService>();
    }
}