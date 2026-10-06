using ChipsPocket.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Infra.Persistence.EntityConfigs;

public class CompletedHandConfig : IEntityTypeConfiguration<CompletedHand>
{
    public void Configure(EntityTypeBuilder<CompletedHand> builder)
    {
        builder.HasOne<Table>()
            .WithMany()
            .HasForeignKey(h => h.TableId);

        builder.HasOne<Seat>()
            .WithMany()
            .HasForeignKey(h => h.BigBlindSeatId);

        builder.HasOne<Seat>()
            .WithMany()
            .HasForeignKey(h => h.SmallBlindSeatId);

        builder.HasOne<Seat>()
            .WithMany()
            .HasForeignKey(h => h.DealerSeatId);
    }
}