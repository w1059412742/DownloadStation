using System.Collections.Generic;

namespace DownloadStation.Server.Dtos.Requests
{
    public class SoftwareUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? OfficialUrl { get; set; }
        public string? CategoryId { get; set; }
        public string? PlatformId { get; set; }
    }
}
