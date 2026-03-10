namespace DownloadStation.Server.Dtos.Requests
{
    public class VersionCreateRequest
    {
        public string SoftwareId { get; set; } = string.Empty;
        public string VersionNumber { get; set; } = string.Empty;
        public string? Changelog { get; set; }
        public string FilePath { get; set; } = string.Empty;
    }
}
