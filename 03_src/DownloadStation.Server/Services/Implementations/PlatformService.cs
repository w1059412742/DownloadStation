using DownloadStation.Server.Data;
using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Dtos.Responses;
using DownloadStation.Server.Models;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Implementations
{
    public class PlatformService : IPlatformService
    {
        private readonly AppDbContext _context;

        public PlatformService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlatformResponse>> GetAllAsync()
        {
            var platforms = await _context.Platforms
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.CreatedAt)
                .ToListAsync();

            return platforms.Select(p => new PlatformResponse
            {
                Id = p.Id,
                Name = p.Name,
                IconClass = p.IconClass,
                ColorHex = p.ColorHex,
                SortOrder = p.SortOrder,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();
        }

        public async Task<PlatformResponse> CreateAsync(PlatformCreateRequest request)
        {
            if (await _context.Platforms.AnyAsync(p => p.Name == request.Name))
                throw new InvalidOperationException($"平台名称 '{request.Name}' 已存在。");

            var platform = new Platform
            {
                Name = request.Name,
                IconClass = string.IsNullOrWhiteSpace(request.IconClass) ? null : request.IconClass,
                ColorHex = string.IsNullOrWhiteSpace(request.ColorHex) ? null : request.ColorHex,
                SortOrder = request.SortOrder,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Platforms.Add(platform);
            await _context.SaveChangesAsync();

            return new PlatformResponse
            {
                Id = platform.Id,
                Name = platform.Name,
                IconClass = platform.IconClass,
                ColorHex = platform.ColorHex,
                SortOrder = platform.SortOrder,
                CreatedAt = platform.CreatedAt,
                UpdatedAt = platform.UpdatedAt
            };
        }

        public async Task<PlatformResponse?> UpdateAsync(string id, PlatformUpdateRequest request)
        {
            var platform = await _context.Platforms.FindAsync(id);
            if (platform == null) return null;

            if (platform.Name != request.Name && await _context.Platforms.AnyAsync(p => p.Name == request.Name))
                throw new InvalidOperationException($"平台名称 '{request.Name}' 已存在记录。");

            platform.Name = request.Name;
            platform.IconClass = string.IsNullOrWhiteSpace(request.IconClass) ? null : request.IconClass;
            platform.ColorHex = string.IsNullOrWhiteSpace(request.ColorHex) ? null : request.ColorHex;
            platform.SortOrder = request.SortOrder;
            platform.UpdatedAt = DateTime.UtcNow;

            _context.Platforms.Update(platform);
            await _context.SaveChangesAsync();

            return new PlatformResponse
            {
                Id = platform.Id,
                Name = platform.Name,
                IconClass = platform.IconClass,
                ColorHex = platform.ColorHex,
                SortOrder = platform.SortOrder,
                CreatedAt = platform.CreatedAt,
                UpdatedAt = platform.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var platform = await _context.Platforms.FindAsync(id);
            if (platform == null) return false;

            _context.Platforms.Remove(platform);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
