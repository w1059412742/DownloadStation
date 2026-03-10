using DownloadStation.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DownloadStation.Server.Data.Configurations
{
    public class SoftwareConfiguration : IEntityTypeConfiguration<Software>
    {
        public void Configure(EntityTypeBuilder<Software> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasMaxLength(16).IsRequired();
            builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
            builder.Property(s => s.Summary).HasMaxLength(500);
            builder.Property(s => s.IconPath).HasMaxLength(500);
            builder.Property(s => s.OfficialUrl).HasMaxLength(500);
            builder.Property(s => s.CategoryId).HasMaxLength(16);
            builder.Property(s => s.PlatformId).HasMaxLength(16);

            // 关系配置
            builder.HasOne(s => s.Platform)
                .WithMany(p => p.Softwares)
                .HasForeignKey(s => s.PlatformId)
                .OnDelete(DeleteBehavior.SetNull);

            // 索引
            builder.HasIndex(s => s.CategoryId);
            builder.HasIndex(s => s.PlatformId);
            builder.HasIndex(s => s.Status); // 用于前台上架查询
        }
    }
}
