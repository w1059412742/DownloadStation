using System;
using DownloadStation.Server.Models.Enums;
using System.Collections.Generic;

namespace DownloadStation.Server.Models
{
    /// <summary>
    /// 软件主要元数据档案，记录软件的基本展示信息以及关联版本。
    /// </summary>
    public class Software
    {
        /// <summary>
        /// 获取或设置 16 位字符的主键。
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 16);

        /// <summary>
        /// 软件的全称。
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 对软件功能的一句话概括总结。
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// 详细特性的富文本内容描述（将采用 Markdown 格式）。
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 缓存或保存的软件高清官方图标相对路径。
        /// </summary>
        public string? IconPath { get; set; }

        /// <summary>
        /// 软件所属公司的官网 URL 链接。
        /// </summary>
        public string? OfficialUrl { get; set; }

        /// <summary>
        /// 本软件隶属分类的外键标示。
        /// </summary>
        public string? CategoryId { get; set; }

        /// <summary>
        /// 上架展示状态：值为 Draft 时不在前台呈现。
        /// </summary>
        public SoftwareStatus Status { get; set; } = SoftwareStatus.Published;

        /// <summary>
        /// 各版本下载历史的一个累计总和缓存值，优化前排展示。在实际下载行为发生后进行异步的叠加统计。
        /// </summary>
        public int TotalDownloads { get; set; } = 0;

        /// <summary>
        /// 纳入系统的标准时间记录。
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最近的更新与调整发生的时间节点。
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 导航属性
        
        /// <summary>
        /// 从属的单一分类。
        /// </summary>
        public Category? Category { get; set; }

        /// <summary>
        /// 本软件所属的单一运行平台（如 Windows, macOS 等）。
        /// </summary>
        public string? PlatformId { get; set; }

        /// <summary>
        /// 导航属性：关联到具体的平台记录。
        /// </summary>
        public Platform? Platform { get; set; }

        /// <summary>
        /// 包含本软件界面的各种宣传图、截屏集合。
        /// </summary>
        public ICollection<SoftwareScreenshot> Screenshots { get; set; } = new List<SoftwareScreenshot>();

        /// <summary>
        /// 管理各个可用下载版的合集资源。
        /// </summary>
        public ICollection<SoftwareVersion> Versions { get; set; } = new List<SoftwareVersion>();
    }
}
