using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class TableChip
{
    public Guid TableId { get; set; }
    public Table Table { get; set; } = null!;

    public Guid TypeId { get; set; }
    public ChipType Type { get; set; } = null!;

    public int Value { get; set; }
}

public class TableChipConfig : IEntityTypeConfiguration<TableChip>
{
    public void Configure(EntityTypeBuilder<TableChip> builder)
    {
        builder.HasKey(c => new { c.TypeId, c.TableId });

        builder.HasOne(c => c.Table)
            .WithMany(t => t.Chips)
            .HasForeignKey(c => c.TableId);

        builder.HasOne(c => c.Type)
            .WithMany()
            .HasForeignKey(c => c.TypeId);
    }
}