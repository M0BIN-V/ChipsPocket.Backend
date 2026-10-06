using ChipsPocket.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChipsPocket.Api.Infra.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<TableMember> TableMembers { get; init; }
    public DbSet<CompletedHand> CompletedHands { get; init; }
    public DbSet<Transaction> Transactions { get; init; }
    public DbSet<Seat> Seats { get; init; }
    public DbSet<Table> Tables { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}