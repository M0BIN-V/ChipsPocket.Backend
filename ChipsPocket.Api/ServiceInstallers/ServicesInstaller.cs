using ChipsPocket.Api.Infra.Persistence.Repositories;
using ChipsPocket.Domain.Contracts;
using ChipsPocket.Domain.Services.HandActionManager;
using ChipsPocket.Domain.Services.HandManager;
using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class ServicesInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services
            .AddSingleton<IActiveHandRepository, ActiveHandRepository>()
            .AddScoped<IHandActionManager, HandActionManager>()
            .AddScoped<ITableRepository, TableRepository>()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped<ISeatRepository, SeatRepository>()
            .AddScoped<IMembersRepository, MembersRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ITransactionRepository, TransactionRepository>()
            .AddScoped<IHandManagerService, HandManagerService>()
            .AddScoped<ICompletedHandRepository, CompletedHandsRepository>();
    }
}