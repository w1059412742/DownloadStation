using System;
using System.Collections.Generic;

namespace DownloadStation.Server.Models
{
    /// <summary>
    /// 软件分类实体类。提供无限层级的树状结构分类。
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 获取或设置 16 位字符的 GUID 主键。
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 16);

        /// <summary>
        /// 获取或设置分类名称。
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 获取或设置父级分类的 ID（可为空，为空则为顶级分类）。
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// 获取或设置排序权重（越小越靠前）。
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// 获取或设置创建时间。
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 获取或设置最后更新时间。
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 导航属性
        
        /// <summary>
        /// 获取或设置父级分类导航属性。
        /// </summary>
        public Category? Parent { get; set; }

        /// <summary>
        /// 获取或设置子分类集合。
        /// </summary>
        public ICollection<Category> Children { get; set; } = new List<Category>();

        /// <summary>
        /// 获取或设置该分类下的所有软件。
        /// </summary>
        public ICollection<Software> Softwares { get; set; } = new List<Software>();
    }
}
