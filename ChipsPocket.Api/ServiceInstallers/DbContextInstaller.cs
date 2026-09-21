using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class DbContextInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite("Data Source=Db/ChipsPocket.db");
        });
    }
}