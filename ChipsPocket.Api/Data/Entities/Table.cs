using ChipsPocket.Api.Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class Table : Entity
{
    private Table()
    {
    }

    public int BigBlindAmount { get; set; }
    public int SmallBlindAmount { get; set; }

    public List<Hand> Hands { get; private set; } = [];

    public User CreatedBy { get; private set; } = null!;
    public string CreatedById { get; private set; } = null!;

    public User Manager { get; set; } = null!;
    public string ManagerId { get; set; } = null!;

    public required string Name { get; set; }
    public List<Seat> Seats { get; private set; } = [];

    public DateTimeOffset CreatedAt { get; private set; }

    public List<TableMember> Members { get; set; } = null!;

    public static Table Create(string name, string creatorId, int smallBlind, int bigBlind)
    {
        var id = Guid.CreateVersion7();

        return new Table
        {
            Members = [],
            SmallBlindAmount = smallBlind,
            BigBlindAmount = bigBlind,
            Id = id,
            CreatedById = creatorId,
            Name = name,
            Seats = Enumerable.Range(1, 10)
                .Select(order => new Seat
                {
                    TableId = id,
                    Order = order
                })
                .ToList(),
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}

public class TableConfig : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasOne(t => t.CreatedBy)
            .WithMany()
            .HasForeignKey(t => t.CreatedById);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.HasIndex(c => new { c.CreatedById, c.Name }, "IX_Tables_CreatedById_Name");

        builder.HasMany(t => t.Seats)
            .WithOne(s => s.Table)
            .HasForeignKey(t => t.TableId);

        builder.Property(x => x.CreatedById)
            .HasMaxLength(255);

        builder.Property(x => x.ManagerId)
            .HasMaxLength(255);


        builder.HasOne(t => t.Manager)
            .WithMany()
            .HasForeignKey(t => t.ManagerId);
    }
}