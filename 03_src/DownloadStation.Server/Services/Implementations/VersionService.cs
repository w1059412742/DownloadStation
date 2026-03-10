using DownloadStation.Server.Data;
using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Dtos.Responses;
using DownloadStation.Server.Models;
using DownloadStation.Server.Models.Enums;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Implementations
{
    public class VersionService : IVersionService
    {
        private readonly AppDbContext _context;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public VersionService(AppDbContext context, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<VersionResponse>> GetBySoftwareIdAsync(string softwareId, bool includeHidden)
        {
            var query = _context.SoftwareVersions
                .Where(v => v.SoftwareId == softwareId)
                .AsQueryable();

            if (!includeHidden)
            {
                query = query.Where(v => v.IsVisible == 1);
            }

            var versions = await query
                .OrderByDescending(v => v.CreatedAt) // 默认时间倒序展示
                .ToListAsync();

            return versions.Select(MapToResponse).ToList();
        }

        public async Task<VersionResponse?> GetByIdAsync(string id)
        {
            var version = await _context.SoftwareVersions.FindAsync(id);
            return version == null ? null : MapToResponse(version);
        }

        public async Task<VersionResponse> CreateAsync(VersionCreateRequest request)
        {
            // 通过文件路径提取出原始文件名。
            // 实际上这里的 FilePath 应该是 NAS 被挂载的某个相对路径的存储指针。
            var fileName = Path.GetFileName(request.FilePath);

            // 读取文件大小 (实际情况可能会在 FileService 或扫描独立解决此步骤再传递)
            long fileSize = 0;
            if (File.Exists(request.FilePath)) 
            {
               fileSize = new FileInfo(request.FilePath).Length;
            }

            var version = new SoftwareVersion
            {
                SoftwareId = request.SoftwareId,
                VersionNumber = request.VersionNumber,
                Changelog = request.Changelog,
                FilePath = request.FilePath,   // "/volume1/Software/Dev/idea-2023.exe" 等
                FileName = fileName,
                FileSize = fileSize,
                HashStatus = HashStatus.Pending, // 压入计算队列待处理
                IsVisible = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.SoftwareVersions.Add(version);
            await _context.SaveChangesAsync();

            return MapToResponse(version);
        }

        public async Task<VersionResponse> UploadAsync(string softwareId, string versionNumber, string? changelog, Microsoft.AspNetCore.Http.IFormFile file)
        {
            var software = await _context.Softwares.FindAsync(softwareId);
            if (software == null) throw new Exception("软件不存在");

            var storagePath = _configuration.GetValue<string>("AppSettings:StorageBasePath") ?? "./SoftwareStorage";
            
            // 为每个软件创建独立文件夹
            var softwareFolder = Path.Combine(storagePath, software.Name.Replace(" ", "_"));
            if (!Directory.Exists(softwareFolder))
            {
                Directory.CreateDirectory(softwareFolder);
            }

            var filePath = Path.Combine(softwareFolder, file.FileName);
            
            // 如果文件已存在，则追加时间戳
            if (File.Exists(filePath))
            {
                filePath = Path.Combine(softwareFolder, $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}");
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var version = new SoftwareVersion
            {
                SoftwareId = softwareId,
                VersionNumber = versionNumber,
                Changelog = changelog,
                FilePath = filePath,
                FileName = Path.GetFileName(filePath),
                FileSize = file.Length,
                HashStatus = HashStatus.Pending,
                IsVisible = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.SoftwareVersions.Add(version);
            await _context.SaveChangesAsync();

            return MapToResponse(version);
        }

        public async Task<VersionResponse?> UpdateAsync(string id, VersionUpdateRequest request)
        {
            var version = await _context.SoftwareVersions.FindAsync(id);
            if (version == null) return null;

            version.VersionNumber = request.VersionNumber;
            version.Changelog = request.Changelog;
            version.UpdatedAt = DateTime.UtcNow;

            _context.SoftwareVersions.Update(version);
            await _context.SaveChangesAsync();

            return MapToResponse(version);
        }

        public async Task<bool> DeleteAsync(string id, bool physicalDelete)
        {
            var version = await _context.SoftwareVersions.FindAsync(id);
            if (version == null) return false;

            if (physicalDelete && File.Exists(version.FilePath))
            {
                try 
                {
                    File.Delete(version.FilePath);
                } 
                catch (Exception) 
                { 
                    // 记录日志，但不阻断，这留给后面的基础服务细化
                }
            }

            _context.SoftwareVersions.Remove(version);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeVisibilityAsync(string id, int isVisible)
        {
            var version = await _context.SoftwareVersions.FindAsync(id);
            if (version == null) return false;

            version.IsVisible = isVisible;
            version.UpdatedAt = DateTime.UtcNow;

            _context.SoftwareVersions.Update(version);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task IncrementDownloadAsync(string id)
        {
            var version = await _context.SoftwareVersions.FindAsync(id);
            if (version == null) return;

            version.DownloadCount++;
            
            // 冗余字段同步给软件本体进行排行筛选使用
            var software = await _context.Softwares.FindAsync(version.SoftwareId);
            if (software != null)
            {
                software.TotalDownloads++;
            }

            await _context.SaveChangesAsync();
        }

        private static VersionResponse MapToResponse(SoftwareVersion version)
        {
            return new VersionResponse
            {
                Id = version.Id,
                SoftwareId = version.SoftwareId,
                VersionNumber = version.VersionNumber,
                Changelog = version.Changelog,
                FileName = version.FileName,
                FilePath = version.FilePath,
                FileSize = version.FileSize,
                HashSHA256 = version.HashSHA256,
                HashStatus = version.HashStatus,
                DownloadCount = version.DownloadCount,
                IsVisible = version.IsVisible,
                CreatedAt = version.CreatedAt,
                UpdatedAt = version.UpdatedAt
            };
        }
    }
}
