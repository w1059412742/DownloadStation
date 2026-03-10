namespace DownloadStation.Server.Dtos.Requests
{
    public class FileBindRequest
    {
        public string FilePath { get; set; } = string.Empty;
        public string SoftwareId { get; set; } = string.Empty;
        public string VersionNumber { get; set; } = string.Empty;
        public string? Changelog { get; set; }
    }
}
