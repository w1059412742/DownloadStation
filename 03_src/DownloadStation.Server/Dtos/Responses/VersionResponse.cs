using DownloadStation.Server.Models.Enums;
using System;

namespace DownloadStation.Server.Dtos.Responses
{
    public class VersionResponse
    {
        public string Id { get; set; } = string.Empty;
        public string SoftwareId { get; set; } = string.Empty;
        public string VersionNumber { get; set; } = string.Empty;
        public string? Changelog { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string? HashSHA256 { get; set; }
        public HashStatus HashStatus { get; set; }
        public int DownloadCount { get; set; }
        public int IsVisible { get; set; }
        public int IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
