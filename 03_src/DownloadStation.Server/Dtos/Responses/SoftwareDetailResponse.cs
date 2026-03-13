using System;
using System.Collections.Generic;
using DownloadStation.Server.Models.Enums;

namespace DownloadStation.Server.Dtos.Responses
{
    public class SoftwareDetailResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? IconPath { get; set; }
        public string? OfficialUrl { get; set; }
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public SoftwareStatus Status { get; set; }
        public int TotalDownloads { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public PlatformResponse? Platform { get; set; }
        public List<TagResponse> Tags { get; set; } = new List<TagResponse>();
        public List<SoftwareScreenshotResponse> Screenshots { get; set; } = new();

    }

    public class SoftwareScreenshotResponse
    {
        public string Id { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
