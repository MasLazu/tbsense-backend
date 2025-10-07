using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Configurations;

public class AiSessionChatConfiguration : IEntityTypeConfiguration<AiSessionChat>
{
    public void Configure(EntityTypeBuilder<AiSessionChat> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.SessionId)
            .IsRequired();

        builder.Property(c => c.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Order)
            .IsRequired();

        builder.Property(c => c.Content)
            .IsRequired();

        builder.HasOne(c => c.AiSession)
            .WithMany(s => s.AiSessionChats)
            .HasForeignKey(c => c.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => new { c.SessionId, c.Order });
    }
}