using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class PlantationConfiguration : IEntityTypeConfiguration<Plantation>
{
    public void Configure(EntityTypeBuilder<Plantation> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.LandAreaHectares)
            .IsRequired();

        builder.Property(p => p.PlantedDate)
            .IsRequired();

        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}