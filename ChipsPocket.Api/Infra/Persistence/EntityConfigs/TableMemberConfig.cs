using ChipsPocket.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Infra.Persistence.EntityConfigs;

public class TableMemberUserConfig : IEntityTypeConfiguration<TableMember>
{
    public void Configure(EntityTypeBuilder<TableMember> builder)
    {
        builder.HasKey(x => new
        {
            x.TableId,
            x.UserId
        });

        builder.Property(x => x.UserId)
            .HasMaxLength(300);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Table>()
            .WithMany()
            .HasForeignKey(x => x.TableId);
    }
}