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
}