using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class UserStack
{
    public Guid Id { get; set; }

    public Table Table { get; set; } = null!;
    public Guid TableId { get; set; }

    public User User { get; set; } = null!;
    public required string UserId { get; set; }

    public List<TableChipCollection> ChipCollections { get; set; } = [];
}

public class UserStackConfig : IEntityTypeConfiguration<UserStack>
{
    public void Configure(EntityTypeBuilder<UserStack> builder)
    {
        builder.HasOne(t => t.Table)
            .WithMany()
            .HasForeignKey(t => t.TableId);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.HasMany(c => c.ChipCollections)
            .WithOne(c => c.UserStack)
            .HasForeignKey(c => c.UserStackId);

        builder.HasIndex(x => new { x.UserId, x.TableId }).IsUnique();

        builder.Property(c => c.UserId)
            .HasMaxLength(300);
    }
}