using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChipsPocket.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Hand> Hands { get; init; }
    public DbSet<Transaction> ChipTransactions { get; init; }
    public DbSet<Seat> Seats { get; init; }
    public DbSet<Table> Tables { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}