using ChipsPocket.Api.Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class Hand : Entity
{
    public Guid TableId { get; set; }
    public Table Table { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public Street CurrentStreet { get; set; }

    public List<ChipTransaction> ChipTransactions { get; set; } = [];

    public Seat BigBlindSeat { get; set; } = null!;
    public Guid BigBlindSeatId { get; set; }

    public Seat SmallBlindSeat { get; set; } = null!;
    public Guid SmallBlindSeatId { get; set; }

    public Seat DealerSeat { get; set; } = null!;
    public Guid DealerSeatId { get; set; }
}

public class HandConfig : IEntityTypeConfiguration<Hand>
{
    public void Configure(EntityTypeBuilder<Hand> builder)
    {
        builder.HasOne(h => h.Table)
            .WithMany(t => t.Hands)
            .HasForeignKey(h => h.TableId);

        builder.HasOne(h => h.BigBlindSeat)
            .WithMany()
            .HasForeignKey(h => h.BigBlindSeatId);

        builder.HasOne(h => h.SmallBlindSeat)
            .WithMany()
            .HasForeignKey(h => h.SmallBlindSeatId);

        builder.HasOne(h => h.DealerSeat)
            .WithMany()
            .HasForeignKey(h => h.DealerSeatId);
    }
}