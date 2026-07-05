using DownloadStation.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DownloadStation.Server.Data.Configurations
{
    public class SoftwareVersionConfiguration : IEntityTypeConfiguration<SoftwareVersion>
    {
        public void Configure(EntityTypeBuilder<SoftwareVersion> builder)
        {
            builder.HasKey(sv => sv.Id);
            builder.Property(sv => sv.Id).HasMaxLength(16).IsRequired();
            builder.Property(sv => sv.SoftwareId).HasMaxLength(16).IsRequired();
            builder.Property(sv => sv.VersionNumber).HasMaxLength(100).IsRequired();
            builder.Property(sv => sv.FileName).HasMaxLength(500).IsRequired();
            builder.Property(sv => sv.FilePath).HasMaxLength(1000).IsRequired();
            builder.Property(sv => sv.HashSHA256).HasMaxLength(64);

            builder.HasOne(sv => sv.Software)
                .WithMany(s => s.Versions)
                .HasForeignKey(sv => sv.SoftwareId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(sv => new { sv.SoftwareId, sv.CreatedAt }).IsDescending();
            builder.HasIndex(sv => sv.SoftwareId)
                .HasFilter("\"IsDefault\" = 1")
                .IsUnique();
            builder.HasIndex(sv => sv.IsVisible);
        }
    }
}
