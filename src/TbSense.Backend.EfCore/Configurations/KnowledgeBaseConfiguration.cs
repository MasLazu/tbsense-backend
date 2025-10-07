using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class KnowledgeBaseConfiguration : IEntityTypeConfiguration<KnowledgeBase>
{
    public void Configure(EntityTypeBuilder<KnowledgeBase> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(k => k.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(k => k.Content)
            .IsRequired();

        builder.HasIndex(k => k.Title);
    }
}