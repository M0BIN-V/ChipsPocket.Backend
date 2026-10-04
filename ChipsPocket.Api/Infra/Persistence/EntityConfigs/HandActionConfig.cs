using ChipsPocket.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Infra.Persistence.EntityConfigs;

public class HandActionConfig : IEntityTypeConfiguration<HandAction>
{
    public void Configure(EntityTypeBuilder<HandAction> builder)
    {
        builder.Property(x => x.Type)
            .HasConversion<string>();

        builder.Property(x => x.Street)
            .HasConversion<string>();

        builder.HasOne<Hand>()
            .WithMany()
            .HasForeignKey(x => x.HandId);
    }
}