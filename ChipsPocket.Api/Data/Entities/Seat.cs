using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class Seat
{
    public Guid Id { get; set; }
    public int Order { get; set; }

    public User? User { get; set; }
    public string? UserId { get; set; }

    public Guid TableId { get; set; }
    public Table Table { get; set; } = null!;
}

public class SeatConfig : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId);
    }
}