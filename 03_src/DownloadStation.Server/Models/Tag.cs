using System;
using System.Collections.Generic;

namespace DownloadStation.Server.Models
{
    /// <summary>
    /// 软件标签，用于对软件进行多维度的分类标注（如：开源、免费、热门等）。
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// 获取或设置 16 位字符的主键。
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 16);

        /// <summary>
        /// 标签名称。
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 标签显示的背景颜色（Hex 格式，如 #FF0000）。
        /// </summary>
        public string? ColorHex { get; set; }

        /// <summary>
        /// 关联的软件集合（多对多）。
        /// </summary>
        public ICollection<Software> Softwares { get; set; } = new List<Software>();

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
