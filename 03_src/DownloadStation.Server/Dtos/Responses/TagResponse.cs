using System;

namespace DownloadStation.Server.Dtos.Responses
{
    /// <summary>
    /// 返回给前端的标签数据响应。
    /// </summary>
    public class TagResponse
    {
        /// <summary>
        /// 16 位字符主键。
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 标签名称。
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 背景颜色的 Hex 字符串。
        /// </summary>
        public string? ColorHex { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
