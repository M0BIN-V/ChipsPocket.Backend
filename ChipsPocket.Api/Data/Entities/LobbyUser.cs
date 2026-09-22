using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class LobbyUser
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public TableLobby Lobby { get; set; } = null!;
    public Guid LobbyId { get; set; }
}

public class LobbyUserConfig : IEntityTypeConfiguration<LobbyUser>
{
    public void Configure(EntityTypeBuilder<LobbyUser> builder)
    {
        builder.HasKey(x => new
        {
            x.LobbyId,
            x.UserId
        });

        builder.Property(x => x.UserId)
            .HasMaxLength(300);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Lobby)
            .WithMany(x => x.LobbyUsers)
            .HasForeignKey(x => x.LobbyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}