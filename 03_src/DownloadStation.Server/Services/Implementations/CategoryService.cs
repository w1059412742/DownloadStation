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
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryTreeResponse>> GetTreeAsync()
        {
            var allCategories = await _context.Categories
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.CreatedAt)
                .ToListAsync();

            var lookup = allCategories.ToLookup(c => c.ParentId);
            return BuildTree(lookup, null);
        }

        private List<CategoryTreeResponse> BuildTree(ILookup<string?, Category> lookup, string? parentId)
        {
            var nodes = new List<CategoryTreeResponse>();
            foreach (var category in lookup[parentId])
            {
                var node = new CategoryTreeResponse
                {
                    Id = category.Id,
                    Name = category.Name,
                    ParentId = category.ParentId,
                    SortOrder = category.SortOrder,
                    Children = BuildTree(lookup, category.Id)
                };
                nodes.Add(node);
            }
            return nodes;
        }

        public async Task<CategoryTreeResponse> CreateAsync(CategoryCreateRequest request)
        {
            var category = new Category
            {
                Name = request.Name,
                ParentId = string.IsNullOrWhiteSpace(request.ParentId) ? null : request.ParentId,
                SortOrder = request.SortOrder,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryTreeResponse
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                SortOrder = category.SortOrder
            };
        }

        public async Task<CategoryTreeResponse?> UpdateAsync(string id, CategoryUpdateRequest request)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;

            // 循环引用检测简易版：不自律会导致死循环，不建议父级选自己。
            if (id == request.ParentId)
                throw new InvalidOperationException("分类的父级不能是自己。");

            category.Name = request.Name;
            category.ParentId = string.IsNullOrWhiteSpace(request.ParentId) ? null : request.ParentId;
            category.SortOrder = request.SortOrder;
            category.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new CategoryTreeResponse
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                SortOrder = category.SortOrder
            };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var category = await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return false;

            if (category.Children.Any())
                throw new InvalidOperationException("该分类下含有子分类，无法直接删除！");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
