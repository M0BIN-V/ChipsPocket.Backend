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

        if (await db.Chips.AnyAsync()) return;

        List<Chip> chips =
        [
            new()
            {
                Name = "White",
                Picture = "white-chip.png",
                Value = 1
            },
            new()
            {
                Name = "Yellow",
                Picture = "yellow-chip.png",
                Value = 2
            },
            new()
            {
                Name = "Red",
                Picture = "red-chip.png",
                Value = 5
            },
            new()
            {
                Name = "Blue",
                Picture = "blue-chip.png",
                Value = 10
            },
            new()
            {
                Name = "Grey",
                Picture = "grey-chip.png",
                Value = 20
            },
            new()
            {
                Name = "Green",
                Picture = "green-chip.png",
                Value = 25
            },
            new()
            {
                Name = "Orange",
                Picture = "orange-chip.png",
                Value = 50
            },
            new()
            {
                Name = "Black",
                Picture = "black-chip.png",
                Value = 100
            },
            new()
            {
                Name = "Pink",
                Picture = "pink-chip.png",
                Value = 250
            },
            new()
            {
                Name = "Purple",
                Picture = "purple-chip.png",
                Value = 500
            },
            new()
            {
                Name = "Yellow",
                Picture = "yellow-chip.png",
                Value = 1000
            },
            new()
            {
                Name = "Light Blue",
                Picture = "light-blue-chip.png",
                Value = 2000
            },
            new()
            {
                Name = "Brown",
                Picture = "brown-chip.png",
                Value = 5000
            }
        ];

        await db.Chips.AddRangeAsync(chips);
        await db.SaveChangesAsync();
    }
}