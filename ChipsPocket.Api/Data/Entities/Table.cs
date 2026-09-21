using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class Table
{
    public Guid Id { get; set; }

    public User CreatedBy { get; set; } = null!;
    public required string CreatedById { get; set; }

    public required string Name { get; set; }
    public required int MaxSeatCount { get; set; }

    public List<Seat> Seats { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
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

        builder.HasIndex(t => t.Name)
            .IsUnique();

        builder.HasMany(t => t.Seats)
            .WithOne(s => s.Table)
            .HasForeignKey(t => t.TableId);
    }
}