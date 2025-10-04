using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class PlantationCoordinateConfiguration : IEntityTypeConfiguration<PlantationCoordinate>
{
    public void Configure(EntityTypeBuilder<PlantationCoordinate> builder)
    {
        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.PlantationId)
            .IsRequired();

        builder.Property(pc => pc.Longitude)
            .IsRequired();

        builder.Property(pc => pc.Latitude)
            .IsRequired();

        builder.HasOne(pc => pc.Plantation)
            .WithMany(p => p.PlantationCoordinates)
            .HasForeignKey(pc => pc.PlantationId);
    }
}