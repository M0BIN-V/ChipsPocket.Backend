using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class Table : Entity
{
    private Table()
    {
    }

    public User CreatedBy { get; private set; } = null!;
    public string CreatedById { get; private set; } = null!;

    public required string Name { get; set; }
    public List<Seat> Seats { get; private set; } = [];

    public DateTimeOffset CreatedAt { get; private set; }

    public List<TableChip> Chips { get; private set; } = [];

    public static Table Create(string name, string creatorId)
    {
        var id = Guid.CreateVersion7();

        return new Table
        {
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

    public void AddChipType(ChipType chipType, int value)
    {
        var tableChip = new TableChip
        {
            TableId = Id,
            Table = this,
            TypeId = chipType.Id,
            Type = chipType,
            Value = value
        };

        Chips.Add(tableChip);
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
    }
}