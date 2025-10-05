using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class TreeConfiguration : IEntityTypeConfiguration<Tree>
{
    public void Configure(EntityTypeBuilder<Tree> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.PlantationId)
            .IsRequired();

        builder.Property(t => t.Longitude)
            .HasPrecision(10, 7);

        builder.Property(t => t.Latitude)
            .HasPrecision(10, 7);

        builder.HasOne(t => t.Plantation)
            .WithMany(p => p.Trees)
            .HasForeignKey(t => t.PlantationId);
    }
}