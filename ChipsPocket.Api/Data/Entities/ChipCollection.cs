using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class ChipCollection
{
    public Guid Id { get; set; }
    public List<CollectionChip> Chips { get; set; } = [];
}

public class ChipCollectionConfig : IEntityTypeConfiguration<ChipCollection>
{
    public void Configure(EntityTypeBuilder<ChipCollection> builder)
    {
        builder.HasMany(c => c.Chips)
            .WithOne(c => c.ChipCollection)
            .HasForeignKey(c => c.ChipCollectionId);
    }
}