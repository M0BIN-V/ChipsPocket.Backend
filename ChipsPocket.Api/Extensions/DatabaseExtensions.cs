namespace ChipsPocket.Api.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();
    }

    public static async Task SeedDataAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        if (await db.ChipTypes.AnyAsync()) return;

        string[] chipNames = ["Red", "Blue", "Green", "Black", "White", "Yello", "Gold", "Silver"];
        var chipAppearances = chipNames.Select(name => new ChipType
        {
            Name = name,
            Picture = $"{name}-chipType.png"
        });

        await db.ChipTypes.AddRangeAsync(chipAppearances);
        await db.SaveChangesAsync();
    }
}