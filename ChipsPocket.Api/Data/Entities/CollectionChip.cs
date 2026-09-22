using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class CollectionChip
{
    public Guid Id { get; set; }

    public Guid ChipId { get; set; }
    public Chip Chip { get; set; } = null!;

    public int Order { get; set; }

    public Guid ChipCollectionId { get; set; }
    public ChipCollection ChipCollection { get; set; } = null!;
}

public class CollectionChipConfig : IEntityTypeConfiguration<CollectionChip>
{
    public void Configure(EntityTypeBuilder<CollectionChip> builder)
    {
        builder.HasKey(c => c.Id);
    }
}