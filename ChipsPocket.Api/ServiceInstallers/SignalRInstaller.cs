using System.Text.Json.Serialization;
using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class SignalRInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        builder.Services
            .AddSignalR()
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddScoped<ITableNotificationPublisher, SignalRTableNotificationPublisher>();
    }
}