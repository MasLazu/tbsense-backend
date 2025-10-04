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

        builder.HasOne(t => t.Plantation)
            .WithMany(p => p.Trees)
            .HasForeignKey(t => t.PlantationId);
    }
}