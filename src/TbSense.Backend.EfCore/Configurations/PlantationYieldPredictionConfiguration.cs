using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class PlantationYieldPredictionConfiguration : IEntityTypeConfiguration<PlantationYieldPrediction>
{
    public void Configure(EntityTypeBuilder<PlantationYieldPrediction> builder)
    {
        builder.HasKey(pyp => pyp.Id);

        builder.Property(pyp => pyp.PlantationId)
            .IsRequired();

        builder.Property(pyp => pyp.ModelId)
            .IsRequired();

        builder.Property(pyp => pyp.Timestamp)
            .IsRequired();

        builder.HasOne(pyp => pyp.Plantation)
            .WithMany(p => p.YieldPredictions)
            .HasForeignKey(pyp => pyp.PlantationId);

        builder.HasOne(pyp => pyp.Model)
            .WithMany(m => m.YieldPredictions)
            .HasForeignKey(pyp => pyp.ModelId);
    }
}