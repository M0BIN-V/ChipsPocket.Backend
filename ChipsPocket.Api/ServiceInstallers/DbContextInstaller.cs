using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class DbContextInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        var dbDirectory = builder.Configuration["Db:Directory"];
        var dbFileName = builder.Configuration["Db:FileName"];

        Console.WriteLine($"ENV Db__Directory = {Environment.GetEnvironmentVariable("Db__Directory")}");
        Console.WriteLine($"CONFIG Db:Directory = {dbDirectory}");
        Console.WriteLine($"ENV Db__FileName = {Environment.GetEnvironmentVariable("Db__FileName")}");
        Console.WriteLine($"CONFIG Db:FileName = {dbFileName}");
        
        // var dbDirectory = builder.Configuration["Db:Directory"] ??
        //                   throw new InvalidOperationException("Db directory is missing.");
        //
        // var dbFileName = builder.Configuration["Db:FileName"] ??
        //                  throw new InvalidOperationException("Db file name is missing.");

        var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, dbDirectory!);

        Directory.CreateDirectory(dataDirectory);

        var databasePath = Path.Combine(dataDirectory, dbFileName!);

        builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlite($"Data Source={databasePath}"); });
    }
}