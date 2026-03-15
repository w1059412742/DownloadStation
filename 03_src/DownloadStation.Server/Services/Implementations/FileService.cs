using DownloadStation.Server.Data;
using DownloadStation.Server.Dtos.Responses;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public FileService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// 扫描未绑定的物理文件。
        /// </summary>
        /// <param name="customPath">可选的自定义扫描路径。</param>
        /// <returns>未绑定文件列表。</returns>
        public async Task<List<UnboundFileResponse>> ScanUnboundFilesAsync(string? customPath = null)
        {
            var basePath = _config.GetValue<string>("AppSettings:StorageBasePath") ?? "./SoftwareStorage";
            
            // 如果提供了自定义路径，则使用自定义路径
            if (!string.IsNullOrWhiteSpace(customPath))
            {
                basePath = customPath;
            }

            if (!Directory.Exists(basePath))
            {
                // 如果是默认路径且不存在，则创建。如果是自定义路径且不存在，则返回空或抛出异常。
                // 这里选择如果是自定义路径且不存在，直接返回空列表，防止创建非法目录。
                if (!string.IsNullOrWhiteSpace(customPath))
                {
                    return new List<UnboundFileResponse>();
                }
                Directory.CreateDirectory(basePath);
            }

            // 获取目录下的所有文件并转换为规范路径 (简化处理，支持一级或全扫, 这里为了性能扫一层文件或者限定深度)
            var allPhysicalFiles = Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories)
                .Select(p => p.Replace("\\", "/")) // 统一斜杠符号应对跨平台
                .ToList();

            // 获取数据库内目前已经在案记录的文件路径
            var boundFiles = await _context.SoftwareVersions
                .Select(v => v.FilePath.Replace("\\", "/"))
                .ToListAsync();

            // 使用 HashSet 优化查找性能
            var boundSet = new HashSet<string>(boundFiles);

            // 过滤那些在硬盘上存在，但是没有经过我们绑定的孤儿体
            var unboundFiles = allPhysicalFiles.Where(f => !boundSet.Contains(f)).ToList();

            var result = new List<UnboundFileResponse>();
            foreach (var file in unboundFiles)
            {
                result.Add(new UnboundFileResponse
                {
                    FileName = Path.GetFileName(file),
                    FilePath = file,
                    Size = new FileInfo(file).Length
                });
            }

            return result;
        }
    }
}
