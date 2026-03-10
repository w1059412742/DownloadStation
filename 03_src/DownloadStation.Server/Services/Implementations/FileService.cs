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

        public async Task<List<UnboundFileResponse>> ScanUnboundFilesAsync()
        {
            var basePath = _config.GetValue<string>("AppSettings:StorageBasePath") ?? "./SoftwareStorage";
            if (!Directory.Exists(basePath))
            {
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
