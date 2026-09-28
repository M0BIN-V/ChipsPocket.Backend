using ChipsPocket.Api.Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class WaitingForAction : Entity
{
    public Guid HandId { get; set; }
    public Hand Hand { get; set; }

    public Seat Seat { get; set; }
    public Guid SeatId { get; set; }

    public WaitingForActionType Type { get; set; }
}

public enum WaitingForActionType
{
    SmallBlind,
    BigBlind,
    BetAction
}

public class WaitingForActionConfig : IEntityTypeConfiguration<WaitingForAction>
{
    public void Configure(EntityTypeBuilder<WaitingForAction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasOne(w => w.Hand)
            .WithOne(h => h.WaitingForAction)
            .HasForeignKey<WaitingForAction>(w => w.HandId);

        builder.Property(h => h.Type)
            .HasConversion<string>();
    }
}