using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class TableTokenServiceInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();

        builder.Services.AddSingleton<ITableJoinTokenService, TableJoinTokenService>();
    }
}