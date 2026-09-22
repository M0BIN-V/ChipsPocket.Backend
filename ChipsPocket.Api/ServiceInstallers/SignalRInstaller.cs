using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class SignalRInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        builder.Services.AddSignalR();

        builder.Services.AddScoped<ITableEventPublisher, SignalRTableEventPublisher>();
    }
}