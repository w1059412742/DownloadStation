using System;

namespace DownloadStation.Server.Dtos.Responses
{
    public class PlatformResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? IconClass { get; set; }
        public string? ColorHex { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
