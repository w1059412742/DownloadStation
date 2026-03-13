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
    /// <summary>
    /// 提供标签生命全周期管理的业务具体实现类。
    /// </summary>
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TagResponse>> GetAllAsync()
        {
            var tags = await _context.Tags
                .OrderBy(t => t.Name)
                .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();

            return tags.Select(t => new TagResponse
            {
                Id = t.Id,
                Name = t.Name,
                ColorHex = t.ColorHex,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList();
        }

        public async Task<TagResponse> CreateAsync(TagCreateRequest request)
        {
            if (await _context.Tags.AnyAsync(t => t.Name == request.Name))
                throw new InvalidOperationException($"标签名称 '{request.Name}' 已存在。");

            var tag = new Tag
            {
                Name = request.Name,
                ColorHex = string.IsNullOrWhiteSpace(request.ColorHex) ? null : request.ColorHex,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return new TagResponse
            {
                Id = tag.Id,
                Name = tag.Name,
                ColorHex = tag.ColorHex,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt
            };
        }

        public async Task<TagResponse?> UpdateAsync(string id, TagUpdateRequest request)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return null;

            if (tag.Name != request.Name && await _context.Tags.AnyAsync(t => t.Name == request.Name))
                throw new InvalidOperationException($"标签名称 '{request.Name}' 与现有标签冲突。");

            tag.Name = request.Name;
            tag.ColorHex = string.IsNullOrWhiteSpace(request.ColorHex) ? null : request.ColorHex;
            tag.UpdatedAt = DateTime.UtcNow;

            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();

            return new TagResponse
            {
                Id = tag.Id,
                Name = tag.Name,
                ColorHex = tag.ColorHex,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return false;

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
