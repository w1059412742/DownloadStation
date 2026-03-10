using System;
using System.Collections.Generic;

namespace DownloadStation.Server.Models
{
    /// <summary>
    /// 支持的操作系统平台实体。
    /// </summary>
    public class Platform
    {
        /// <summary>
        /// 获取或设置 16 位字符的 GUID 主键。
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 16);

        /// <summary>
        /// 获取或设置平台名称。
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 获取或设置平台图标对应的 CSS 类或自定义标识符。
        /// </summary>
        public string? IconClass { get; set; }

        /// <summary>
        /// 获取或设置平台呈现的背景颜色 (通常形如 #0078D4)
        /// </summary>
        public string? ColorHex { get; set; }

        /// <summary>
        /// 获取或设置用于界面的显示排序权重（越小越在前）。
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// 平台的录入时间。
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 平台的最后更新时间。
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 导航属性
        
        /// <summary>
        /// 获取归属于此平台的所有软件。
        /// </summary>
        public ICollection<Software> Softwares { get; set; } = new List<Software>();
    }
}
