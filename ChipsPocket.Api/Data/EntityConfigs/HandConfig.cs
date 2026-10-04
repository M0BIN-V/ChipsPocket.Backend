using ChipsPocket.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class HandConfig : IEntityTypeConfiguration<Hand>
{
    public void Configure(EntityTypeBuilder<Hand> builder)
    {
        builder.HasOne<Table>()
            .WithMany(t => t.Hands)
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