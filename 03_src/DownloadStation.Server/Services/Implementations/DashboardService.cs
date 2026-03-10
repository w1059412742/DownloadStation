using DownloadStation.Server.Data;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetStatisticsAsync()
        {
            var softwareCount = await _context.Softwares.CountAsync();
            var versionCount = await _context.SoftwareVersions.CountAsync();
            var categoryCount = await _context.Categories.CountAsync();
            var platformCount = await _context.Platforms.CountAsync();
            
            // 累加总下载量
            var totalDownloads = await _context.SoftwareVersions.SumAsync(v => v.DownloadCount);

            return new
            {
                SoftwareCount = softwareCount,
                VersionCount = versionCount,
                CategoryCount = categoryCount,
                PlatformCount = platformCount,
                TotalDownloads = totalDownloads
            };
        }
    }
}
