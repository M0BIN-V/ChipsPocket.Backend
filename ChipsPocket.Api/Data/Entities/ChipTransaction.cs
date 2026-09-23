using ChipsPocket.Api.Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class ChipTransaction : Entity
{
    public required Guid TableId { get; init; }
    public Table Table { get; private set; } = null!;

    public string? FromUserId { get; private set; }
    public User? FromUser { get; private set; }

    public string? ToUserId { get; private set; }
    public User? ToUser { get; private set; }

    public Guid? FromPotId { get; private set; }
    public Pot? FromPot { get; private set; }

    public Guid? ToPotId { get; private set; }
    public Pot? ToPot { get; private set; }

    public List<TransactionChip> Chips { get; } = [];

    internal void SetFromUser(string userId)
    {
        FromUserId = userId;
    }

    internal void SetFromPot(Guid potId)
    {
        FromPotId = potId;
    }

    internal void SetToUser(string userId)
    {
        ToUserId = userId;
    }

    internal void SetToPot(Guid potId)
    {
        ToPotId = potId;
    }

    internal bool HasSource()
    {
        return FromUserId is not null || FromPotId is not null;
    }

    internal bool HasDestination()
    {
        return ToUserId is not null || ToPotId is not null;
    }

    public int GetValue()
    {
        return Chips.Sum(x => x.GetValue());
    }
}

public class ChipTransactionConfig : IEntityTypeConfiguration<ChipTransaction>
{
    public void Configure(EntityTypeBuilder<ChipTransaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Table)
            .WithMany(t=>t.Transactions)
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

        builder.HasMany(x => x.Chips)
            .WithOne(x => x.Transaction)
            .HasForeignKey(x => x.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}