using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Services;
using ChipsPocket.Domain.Services.HandManager;
using ChipsPocket.Domain.Services.UserStack;
using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class ServicesInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services
            .AddScoped<IUserStackService, UserStackService>()
            .AddScoped<ITableRepository, TableRepository>()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped<ISeatRepository, SeatRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ITransactionRepository, TransactionRepository>()
            .AddScoped<IHandManagerService, HandManagerService>()
            .AddScoped<IHandRepository, HandRepository>();
    }
}