using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class ChipType : Entity
{
    public required string Name { get; set; }
    public required string Picture { get; set; }
}

public class ChipTypeConfig : IEntityTypeConfiguration<ChipType>
{
    public void Configure(EntityTypeBuilder<ChipType> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name)
            .HasMaxLength(255);
        
        builder.Property(c => c.Picture)
            .HasMaxLength(255);
    }
}