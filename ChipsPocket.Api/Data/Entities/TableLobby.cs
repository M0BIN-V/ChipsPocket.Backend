using ChipsPocket.Api.Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Data.Entities;

public class TableLobby : Entity
{
    public Guid TableId { get; set; }
    public Table Table { get; set; } = null!;

    public List<LobbyUser> LobbyUsers { get; set; } = [];
}

public class TableLobbyConfig : IEntityTypeConfiguration<TableLobby>
{
    public void Configure(EntityTypeBuilder<TableLobby> builder)
    {
        builder.HasMany(l => l.LobbyUsers)
            .WithOne(l => l.Lobby)
            .HasForeignKey(l => l.LobbyId);
    }
}