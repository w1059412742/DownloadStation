namespace DownloadStation.Server.Dtos.Requests
{
    public class VersionUpdateRequest
    {
        public string VersionNumber { get; set; } = string.Empty;
        public string? Changelog { get; set; }
    }
}
