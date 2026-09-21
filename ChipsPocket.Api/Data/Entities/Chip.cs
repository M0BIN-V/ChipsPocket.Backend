using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class Chip
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string PictureFileName { get; set; }
    public int Value { get; set; }
}

public class ChipConfig : IEntityTypeConfiguration<Chip>
{
    public void Configure(EntityTypeBuilder<Chip> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name)
            .HasMaxLength(255);

        builder.Property(c => c.PictureFileName)
            .HasMaxLength(255);
    }
}