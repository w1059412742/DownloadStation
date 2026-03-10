using DownloadStation.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DownloadStation.Server.Data.Configurations
{
    public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasMaxLength(16).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(50).IsRequired();
            builder.Property(p => p.IconClass).HasMaxLength(100);
            builder.Property(p => p.ColorHex).HasMaxLength(7);

            builder.HasIndex(p => p.Name).IsUnique(); // 平台名需唯一
        }
    }
}
