using DownloadStation.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DownloadStation.Server.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasMaxLength(16).IsRequired();
            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.ParentId).HasMaxLength(16);

            // 自引用：父子关系
            builder.HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 软件关联
            builder.HasMany(c => c.Softwares)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // 索引优化递归查询
            builder.HasIndex(c => c.ParentId);
        }
    }
}
