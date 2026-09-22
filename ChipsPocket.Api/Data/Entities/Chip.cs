using ChipsPocket.Api.Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class Chip : Entity
{
    public required string Name { get; set; }
    public required string Picture { get; set; }

    public required int Value { get; set; }
}

public class ChipTypeConfig : IEntityTypeConfiguration<Chip>
{
    public void Configure(EntityTypeBuilder<Chip> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(255);

        builder.Property(c => c.Picture)
            .HasMaxLength(255);
    }
}