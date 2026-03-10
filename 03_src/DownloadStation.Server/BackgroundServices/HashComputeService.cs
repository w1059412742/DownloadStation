using DownloadStation.Server.Data;
using DownloadStation.Server.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadStation.Server.BackgroundServices
{
    /// <summary>
    /// 后台长效作业：扫描挂起的实体版本记录，通过流式机制静默计算文件的 SHA256 数据防篡改并回填库内。
    /// </summary>
    public class HashComputeService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HashComputeService> _logger;

        public HashComputeService(IServiceProvider serviceProvider, ILogger<HashComputeService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("文件哈希流式提取计算后台队列已启动！");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // 检索一条最早的尚未计算任务锁定
                    var pendingVersion = await dbContext.SoftwareVersions
                        .Where(v => v.HashStatus == HashStatus.Pending)
                        .OrderBy(v => v.CreatedAt)
                        .FirstOrDefaultAsync(stoppingToken);

                    if (pendingVersion != null)
                    {
                        pendingVersion.HashStatus = HashStatus.Computing;
                        pendingVersion.UpdatedAt = DateTime.UtcNow;
                        await dbContext.SaveChangesAsync(stoppingToken);

                        if (!File.Exists(pendingVersion.FilePath))
                        {
                            pendingVersion.HashStatus = HashStatus.Failed;
                            pendingVersion.HashSHA256 = "解析失败: 游离的文件实体不存在（或遭遇阻断）";
                            _logger.LogWarning($"文件未能发现: {pendingVersion.FilePath}");
                        }
                        else
                        {
                            try
                            {
                                using var sha256 = SHA256.Create();
                                using var fileStream = File.OpenRead(pendingVersion.FilePath);
                                
                                // 流式分块读，避免单体积 OOM
                                var hashBytes = await sha256.ComputeHashAsync(fileStream, stoppingToken);
                                pendingVersion.HashSHA256 = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                                pendingVersion.HashStatus = HashStatus.Done;

                                _logger.LogInformation($"文件 {pendingVersion.FileName} Hash256 提取构建完成。");
                            }
                            catch (Exception ex)
                            {
                                pendingVersion.HashStatus = HashStatus.Failed;
                                pendingVersion.HashSHA256 = "解析遇到异常！";
                                _logger.LogError(ex, $"哈希提炼失败: {pendingVersion.FilePath}");
                            }
                        }

                        pendingVersion.UpdatedAt = DateTime.UtcNow;
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                    else
                    {
                        // 队列全空，让出 CPU 给主事务，休眠5秒钟
                        await Task.Delay(5000, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "后台哈希队列运行遭受到了未捕获的冲击错位，挂起中...");
                    await Task.Delay(10000, stoppingToken);
                }
            }
        }
    }
}
