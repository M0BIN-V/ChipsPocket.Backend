using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class TableChipCollection : Entity
{
    public Guid UserStackId { get; set; } 
    public UserStack UserStack { get; set; } = null!;
    public List<CollectionTableChip> CollectionChips { get; set; } = [];
}

public class TableChipCollectionConfig : IEntityTypeConfiguration<TableChipCollection>
{
    public void Configure(EntityTypeBuilder<TableChipCollection> builder)
    {


        builder.HasMany(c => c.CollectionChips)
            .WithOne(c => c.TableChipCollection)
            .HasForeignKey(c => c.TableChipCollectionId);
    }
}