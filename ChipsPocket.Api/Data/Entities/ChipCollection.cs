using ChipsPocket.Api.Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class ChipCollection : Entity
{
    public Guid UserStackId { get; set; }
    public UserStack UserStack { get; set; } = null!;

    public List<CollectionChip> Chips { get; set; } = [];
}

public class TableChipCollectionConfig : IEntityTypeConfiguration<ChipCollection>
{
    public void Configure(EntityTypeBuilder<ChipCollection> builder)
    {
        builder.HasMany(c => c.Chips)
            .WithOne(c => c.ChipCollection)
            .HasForeignKey(c => c.ChipCollectionId);
    }
}