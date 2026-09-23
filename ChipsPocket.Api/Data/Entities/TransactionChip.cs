using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class TransactionChip
{
    public Guid TransactionId { get; private set; }
    public ChipTransaction Transaction { get; private set; } = null!;

    public Guid ChipId { get; set; }
    public Chip Chip { get; set; } = null!;

    public int ChipCount { get; set; }

    public int GetValue()
    {
        return Chip.Value * ChipCount;
    }
}

public class TransactionChipConfig : IEntityTypeConfiguration<TransactionChip>
{
    public void Configure(EntityTypeBuilder<TransactionChip> builder)
    {
        builder.HasKey(x => new
        {
            x.TransactionId,
            x.ChipId
        });

        builder.Property(x => x.ChipCount)
            .IsRequired();

        builder.HasOne(x => x.Transaction)
            .WithMany(x => x.Chips)
            .HasForeignKey(x => x.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Chip)
            .WithMany()
            .HasForeignKey(x => x.ChipId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}