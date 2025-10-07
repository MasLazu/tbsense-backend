using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class AiSessionConfiguration : IEntityTypeConfiguration<AiSession>
{
    public void Configure(EntityTypeBuilder<AiSession> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.LastActivityAt)
            .IsRequired();

        builder.HasMany(a => a.AiSessionChats)
            .WithOne(c => c.AiSession)
            .HasForeignKey(c => c.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}