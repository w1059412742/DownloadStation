using System;

namespace DownloadStation.Server.Models
{
    /// <summary>
    /// 各个软件的画廊相册资源，承载实际系统截图，附带显示顺序等排列机制。
    /// </summary>
    public class SoftwareScreenshot
    {
        /// <summary>
        /// 相册实体自带的唯一自增或通用全局标示（主键）。
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 16);

        /// <summary>
        /// 具体从属于哪个软件。
        /// </summary>
        public string SoftwareId { get; set; } = string.Empty;

        /// <summary>
        /// 指向磁盘或云端的直观图片位置（例如 /uploads/screenshots/...）
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// 如果存在多图需求，依赖此字段按升序提供画廊的幻灯片浏览结构。
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// 具体这张图的记录发生时间。
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 导航：依附于展示页面主体。
        /// </summary>
        public Software? Software { get; set; }
    }
}
