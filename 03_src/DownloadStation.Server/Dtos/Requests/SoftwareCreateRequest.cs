using System.Collections.Generic;

namespace DownloadStation.Server.Dtos.Requests
{
    public class SoftwareCreateRequest
    {
        /// <summary>
        /// 软件全称。
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 简述。
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// 详细描述。
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 软件图标路径。
        /// </summary>
        public string? IconPath { get; set; }

        /// <summary>
        /// 官方网址。
        /// </summary>
        public string? OfficialUrl { get; set; }

        /// <summary>
        /// 分类ID。
        /// </summary>
        public string? CategoryId { get; set; }

        /// <summary>
        /// 平台ID。
        /// </summary>
        public string? PlatformId { get; set; }
    }
}
