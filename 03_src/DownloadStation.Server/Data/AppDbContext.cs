using DownloadStation.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DownloadStation.Server.Data
{
    /// <summary>
    /// 数据上下文服务，统一接入 SQLite 处理。
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Platform> Platforms { get; set; } = null!;
        public DbSet<Software> Softwares { get; set; } = null!;
        public DbSet<SoftwareScreenshot> SoftwareScreenshots { get; set; } = null!;
        public DbSet<SoftwareVersion> SoftwareVersions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // 自动加载位于 Data/Configurations 下当前程序集中定义的所有 EntityTypeConfiguration 配置
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
