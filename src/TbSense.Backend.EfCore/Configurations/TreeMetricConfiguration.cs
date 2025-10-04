using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class TreeMetricConfiguration : IEntityTypeConfiguration<TreeMetric>
{
    public void Configure(EntityTypeBuilder<TreeMetric> builder)
    {
        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.TreeId)
            .IsRequired();

        builder.Property(tm => tm.SoilMoisture)
            .IsRequired();

        builder.Property(tm => tm.SoilTemperature)
            .IsRequired();

        builder.Property(tm => tm.AirTemperature)
            .IsRequired();

        builder.Property(tm => tm.Timestamp)
            .IsRequired();

        builder.HasOne(tm => tm.Tree)
            .WithMany(t => t.Metrics)
            .HasForeignKey(tm => tm.TreeId);
    }
}