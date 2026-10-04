using ChipsPocket.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class ChipTransactionConfig : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Table)
            .WithMany()
            .HasForeignKey(x => x.TableId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.FromUser)
            .WithMany()
            .HasForeignKey(x => x.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToUser)
            .WithMany()
            .HasForeignKey(x => x.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FromPot)
            .WithMany()
            .HasForeignKey(x => x.FromPotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToPot)
            .WithMany()
            .HasForeignKey(x => x.ToPotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}