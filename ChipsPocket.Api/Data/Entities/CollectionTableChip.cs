using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class CollectionTableChip
{
    public Guid Id { get; set; }

    public Guid ChipTypeId { get; set; }
    public Guid TableId { get; set; }
    public TableChip TableChip { get; set; } = null!;

    public int Order { get; set; }

    public Guid TableChipCollectionId { get; set; }
    public TableChipCollection TableChipCollection { get; set; } = null!;
}

public class CollectionChipConfig : IEntityTypeConfiguration<CollectionTableChip>
{
    public void Configure(EntityTypeBuilder<CollectionTableChip> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.TableChip)
            .WithMany()
            .HasForeignKey(x => new
            {
                x.ChipTypeId,
                x.TableId
            });
    }
}