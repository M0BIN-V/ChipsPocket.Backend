using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class DbContextInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "DB");

        Directory.CreateDirectory(dataDirectory);

        var databasePath = Path.Combine(dataDirectory, "ChipsPocket.db");

        builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlite($"Data Source={databasePath}"); });
    }
}