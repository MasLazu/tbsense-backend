using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.TrainingStatus)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(m => m.IsUsed)
            .IsRequired();

        builder.Property(m => m.TrainingDataStart)
            .IsRequired();

        builder.Property(m => m.TrainingDataEnd)
            .IsRequired();

        builder.Property(m => m.Accuracy)
            .IsRequired(false);

        builder.Property(m => m.MAE)
            .IsRequired(false);

        builder.Property(m => m.RMSE)
            .IsRequired(false);

        builder.Property(m => m.R2Score)
            .IsRequired(false);

        builder.HasMany(m => m.YieldPredictions)
            .WithOne(pyp => pyp.Model)
            .HasForeignKey(pyp => pyp.ModelId);
    }
}