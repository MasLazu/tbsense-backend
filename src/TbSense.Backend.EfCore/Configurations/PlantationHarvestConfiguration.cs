using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class PlantationHarvestConfiguration : IEntityTypeConfiguration<PlantationHarvest>
{
    public void Configure(EntityTypeBuilder<PlantationHarvest> builder)
    {
        builder.HasKey(ph => ph.Id);

        builder.Property(ph => ph.PlantationId)
            .IsRequired();

        builder.Property(ph => ph.YieldKg)
            .IsRequired();

        builder.Property(ph => ph.HarvestDate)
            .IsRequired();

        builder.HasOne(ph => ph.Plantation)
            .WithMany(p => p.Harvests)
            .HasForeignKey(ph => ph.PlantationId);
    }
}