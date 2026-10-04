using ChipsPocket.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChipsPocket.Api.Infra.Persistence.EntityConfigs;

public class TableConfig : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.CreatedById);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.HasIndex(c => new { c.CreatedById, c.Name }, "IX_Tables_CreatedById_Name");

        builder.HasMany(t => t.Seats)
            .WithOne()
            .HasForeignKey(t => t.TableId);

        builder.Property(x => x.CreatedById)
            .HasMaxLength(255);

        builder.Property(x => x.ManagerId)
            .HasMaxLength(255);

        builder.HasOne(t => t.CreatedBy)
            .WithMany()
            .HasForeignKey(t => t.CreatedById);


        builder.HasOne(t => t.Manager)
            .WithMany()
            .HasForeignKey(t => t.ManagerId);

        builder.Ignore(t => t.BigBlindAmount);
    }
}