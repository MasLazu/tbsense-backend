using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class SystemPromptConfiguration : IEntityTypeConfiguration<SystemPrompt>
{
    public void Configure(EntityTypeBuilder<SystemPrompt> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Prompt)
            .IsRequired();

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true);
    }
}