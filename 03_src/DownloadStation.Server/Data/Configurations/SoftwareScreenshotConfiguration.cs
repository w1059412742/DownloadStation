using DownloadStation.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DownloadStation.Server.Data.Configurations
{
    public class SoftwareScreenshotConfiguration : IEntityTypeConfiguration<SoftwareScreenshot>
    {
        public void Configure(EntityTypeBuilder<SoftwareScreenshot> builder)
        {
            builder.HasKey(ss => ss.Id);
            builder.Property(ss => ss.Id).HasMaxLength(16).IsRequired();
            builder.Property(ss => ss.SoftwareId).HasMaxLength(16).IsRequired();
            builder.Property(ss => ss.FilePath).HasMaxLength(500).IsRequired();

            builder.HasOne(ss => ss.Software)
                .WithMany(s => s.Screenshots)
                .HasForeignKey(ss => ss.SoftwareId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
