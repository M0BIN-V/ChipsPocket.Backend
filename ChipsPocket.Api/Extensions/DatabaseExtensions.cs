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
                Picture = "white-chip",
                Value = 1
            },
            new()
            {
                Name = "Yellow",
                Picture = "yellow-chip",
                Value = 2
            },
            new()
            {
                Name = "Red",
                Picture = "red-chip",
                Value = 5
            },
            new()
            {
                Name = "Blue",
                Picture = "blue-chip",
                Value = 10
            },
            new()
            {
                Name = "Grey",
                Picture = "grey-chip",
                Value = 20
            },
            new()
            {
                Name = "Green",
                Picture = "green-chip",
                Value = 25
            },
            new()
            {
                Name = "Orange",
                Picture = "orange-chip",
                Value = 50
            },
            new()
            {
                Name = "Black",
                Picture = "black-chip",
                Value = 100
            },
            new()
            {
                Name = "Pink",
                Picture = "pink-chip",
                Value = 250
            },
            new()
            {
                Name = "Purple",
                Picture = "purple-chip",
                Value = 500
            },
            new()
            {
                Name = "Yellow",
                Picture = "yellow-chip",
                Value = 1000
            },
            new()
            {
                Name = "Light Blue",
                Picture = "light-blue-chip",
                Value = 2000
            },
            new()
            {
                Name = "Brown",
                Picture = "brown-chip",
                Value = 5000
            }
        ];

        await db.Chips.AddRangeAsync(chips);
        await db.SaveChangesAsync();
    }
}