namespace DownloadStation.Server.Dtos.Requests
{
    /// <summary>
    /// 创建新标签的请求负载。
    /// </summary>
    public class TagCreateRequest
    {
        /// <summary>
        /// 标签名称。
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 背景颜色的 Hex 字符串。
        /// </summary>
        public string? ColorHex { get; set; }
    }

    /// <summary>
    /// 修改现有标签的请求负载。
    /// </summary>
    public class TagUpdateRequest : TagCreateRequest
    {
    }
}
