using DownloadStation.Server.Data;
using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Dtos.Responses;
using DownloadStation.Server.Models;
using DownloadStation.Server.Models.Enums;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Implementations
{
    public class SoftwareService : ISoftwareService
    {
        private readonly AppDbContext _context;

        public SoftwareService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<SoftwareListResponse>> GetPagedListAsync(
            string? categoryId, string? platformId, string? keyword,
            string? sortBy, bool includeDrafts, int page, int pageSize)
        {
            var query = _context.Softwares
                .Include(s => s.Category)
                .Include(s => s.Platform)
                .Include(s => s.Tags)
                .AsQueryable();

            if (!includeDrafts)
            {
                query = query.Where(s => s.Status == SoftwareStatus.Published);
            }

            categoryId = string.IsNullOrWhiteSpace(categoryId) ? null : categoryId;
            platformId = string.IsNullOrWhiteSpace(platformId) ? null : platformId;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(s => s.Name.Contains(keyword) || (s.Summary != null && s.Summary.Contains(keyword)));
            }

            if (categoryId != null)
            {
                // 递归查找子分类，为了演示起见这里简单使用基于内存或者假设只有一级
                // 严谨做法需要独立提取其所有后代项。这里简化处理为包含本类
                var targetCategories = new List<string> { categoryId };

                // 获取所有分类，在内存中构建关系
                var allCategories = await _context.Categories.AsNoTracking().ToListAsync();
                targetCategories.AddRange(GetDescendantCategoryIds(allCategories, categoryId));

                query = query.Where(s => s.CategoryId != null && targetCategories.Contains(s.CategoryId));
            }

            if (platformId != null)
            {
                query = query.Where(s => s.PlatformId == platformId);
            }

            // 排序逻辑
            if (sortBy?.ToLower() == "popular")
            {
                query = query.OrderByDescending(s => s.TotalDownloads).ThenByDescending(s => s.CreatedAt);
            }
            else
            {
                // 默认最新更新
                query = query.OrderByDescending(s => s.UpdatedAt).ThenByDescending(s => s.CreatedAt);
            }

            Console.WriteLine($"[GetPagedListAsync] categoryId: {categoryId}, platformId: {platformId}, includeDrafts: {includeDrafts}");
            var totalCount = await query.CountAsync();
            Console.WriteLine($"[GetPagedListAsync] TotalCount found: {totalCount}");
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var mappedItems = items.Select(s => new SoftwareListResponse
            {
                Id = s.Id,
                Name = s.Name,
                Summary = s.Summary,
                IconPath = s.IconPath,
                CategoryName = s.Category?.Name,
                Status = s.Status,
                TotalDownloads = s.TotalDownloads,
                UpdatedAt = s.UpdatedAt,
                Platform = s.Platform == null ? null : new PlatformResponse
                {
                    Id = s.Platform.Id,
                    Name = s.Platform.Name,
                    IconClass = s.Platform.IconClass,
                    ColorHex = s.Platform.ColorHex
                },
                Tags = s.Tags.Select(t => new TagResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    ColorHex = t.ColorHex
                }).ToList()
            });

            return new PagedResult<SoftwareListResponse>
            {
                Items = mappedItems,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        private IEnumerable<string> GetDescendantCategoryIds(List<Category> all, string parentId)
        {
            var children = all.Where(c => c.ParentId == parentId).Select(c => c.Id).ToList();
            var descendants = new List<string>(children);
            foreach (var child in children)
            {
                descendants.AddRange(GetDescendantCategoryIds(all, child));
            }
            return descendants;
        }

        public async Task<SoftwareDetailResponse?> GetByIdAsync(string id)
        {
            var software = await _context.Softwares
                .Include(s => s.Category)
                .Include(s => s.Platform)
                .Include(s => s.Screenshots)
                .Include(s => s.Tags)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (software == null) return null;

            return new SoftwareDetailResponse
            {
                Id = software.Id,
                Name = software.Name,
                Summary = software.Summary,
                Description = software.Description,
                IconPath = software.IconPath,
                OfficialUrl = software.OfficialUrl,
                CategoryId = software.CategoryId,
                CategoryName = software.Category?.Name,
                Status = software.Status,
                TotalDownloads = software.TotalDownloads,
                CreatedAt = software.CreatedAt,
                UpdatedAt = software.UpdatedAt,
                Platform = software.Platform == null ? null : new PlatformResponse
                {
                    Id = software.Platform.Id,
                    Name = software.Platform.Name,
                    IconClass = software.Platform.IconClass,
                    ColorHex = software.Platform.ColorHex
                },
                Screenshots = software.Screenshots.OrderBy(x => x.SortOrder).Select(ss => new SoftwareScreenshotResponse
                {
                    Id = ss.Id,
                    FilePath = ss.FilePath,
                    SortOrder = ss.SortOrder
                }).ToList(),
                Tags = software.Tags.Select(t => new TagResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    ColorHex = t.ColorHex,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                }).ToList()
            };
        }

        public async Task<SoftwareDetailResponse> CreateAsync(SoftwareCreateRequest request)
        {
            var software = new Software
            {
                Name = request.Name,
                Summary = request.Summary,
                Description = request.Description,
                IconPath = request.IconPath,
                OfficialUrl = request.OfficialUrl,
                CategoryId = string.IsNullOrWhiteSpace(request.CategoryId) ? null : request.CategoryId,
                PlatformId = string.IsNullOrWhiteSpace(request.PlatformId) ? null : request.PlatformId,
                Status = SoftwareStatus.Draft, // 默认下架草稿
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Softwares.Add(software);
            
            // 处理标签关联
            if (request.TagIds != null && request.TagIds.Any())
            {
                var tags = await _context.Tags.Where(t => request.TagIds.Contains(t.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    software.Tags.Add(tag);
                }
            }

            await _context.SaveChangesAsync();

            return await GetByIdAsync(software.Id) ?? throw new Exception("创建软件失败。");
        }

        public async Task<SoftwareDetailResponse?> UpdateAsync(string id, SoftwareUpdateRequest request)
        {
            var software = await _context.Softwares.FirstOrDefaultAsync(s => s.Id == id);

            if (software == null) return null;

            software.Name = request.Name;
            software.Summary = request.Summary;
            software.Description = request.Description;
            software.IconPath = request.IconPath;
            software.OfficialUrl = request.OfficialUrl;
            software.CategoryId = string.IsNullOrWhiteSpace(request.CategoryId) ? null : request.CategoryId;
            software.PlatformId = string.IsNullOrWhiteSpace(request.PlatformId) ? null : request.PlatformId;
            software.UpdatedAt = DateTime.UtcNow;

            _context.Softwares.Update(software);

            // 处理标签增量更新 (先加载现有，再清理，最后重新绑定新标签)
            await _context.Entry(software).Collection(s => s.Tags).LoadAsync();
            software.Tags.Clear();
            if (request.TagIds != null && request.TagIds.Any())
            {
                var tags = await _context.Tags.Where(t => request.TagIds.Contains(t.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    software.Tags.Add(tag);
                }
            }

            await _context.SaveChangesAsync();


            return await GetByIdAsync(software.Id);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var software = await _context.Softwares
                .Include(s => s.Versions)
                .Include(s => s.Screenshots)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (software == null) return false;

            // 删除物理文件 - 版本安装包及其可能的日志附件
            foreach (var version in software.Versions)
            {
                if (!string.IsNullOrEmpty(version.FilePath) && System.IO.File.Exists(version.FilePath))
                {
                    try { System.IO.File.Delete(version.FilePath); } catch (Exception) { /* 忽略文件锁定错 */ }
                }
            }

            // 删除物理文件 - 图文说明/截图
            foreach (var ss in software.Screenshots)
            {
                if (!string.IsNullOrEmpty(ss.FilePath))
                {
                    // 假设直接存的是绝对路径或是可解析的相对路径，我们尽量结合环境变量。
                    // 但通常数据库存的是 URL（比如 /uploads/images/...）这需要转换。
                    // 为了简单起见，如果包含 /uploads 且没有实现完整虚拟路径映射：
                    try
                    {
                        var localPath = ss.FilePath.StartsWith("/uploads")
                            ? System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), ss.FilePath.TrimStart('/'))
                            : ss.FilePath;
                        if (System.IO.File.Exists(localPath)) System.IO.File.Delete(localPath);
                    }
                    catch (Exception) { }
                }
            }

            _context.SoftwareVersions.RemoveRange(software.Versions);
            _context.SoftwareScreenshots.RemoveRange(software.Screenshots);
            _context.Softwares.Remove(software);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatusAsync(string id, int status)
        {
            var software = await _context.Softwares.FindAsync(id);
            if (software == null) return false;

            software.Status = (SoftwareStatus)status;
            software.UpdatedAt = DateTime.UtcNow;

            _context.Softwares.Update(software);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// 检查同平台下是否存在同名软件（排除指定 ID 的记录，用于编辑时排除自身）。
        /// </summary>
        /// <param name="name">软件名称。</param>
        /// <param name="platformId">平台 ID。</param>
        /// <param name="excludeId">需要排除的软件 ID。</param>
        /// <returns>是否已存在。</returns>
        public async Task<bool> CheckNameExistsAsync(string name, string platformId, string? excludeId)
        {
            var query = _context.Softwares
                .Where(s => s.Name == name && s.PlatformId == platformId);

            if (!string.IsNullOrWhiteSpace(excludeId))
            {
                query = query.Where(s => s.Id != excludeId);
            }

            return await query.AnyAsync();
        }
    }
}
