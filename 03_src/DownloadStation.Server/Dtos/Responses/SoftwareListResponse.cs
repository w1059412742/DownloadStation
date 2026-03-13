using System;
using System.Collections.Generic;
using DownloadStation.Server.Models.Enums;

namespace DownloadStation.Server.Dtos.Responses
{
    public class SoftwareListResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? IconPath { get; set; }
        public string? CategoryName { get; set; }
        public SoftwareStatus Status { get; set; }
        public int TotalDownloads { get; set; }
        public DateTime UpdatedAt { get; set; }
        public PlatformResponse? Platform { get; set; }
        public List<TagResponse> Tags { get; set; } = new List<TagResponse>();

    }
}
