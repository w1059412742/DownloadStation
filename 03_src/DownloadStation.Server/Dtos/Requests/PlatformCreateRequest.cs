namespace DownloadStation.Server.Dtos.Requests
{
    public class PlatformCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? IconClass { get; set; }
        public string? ColorHex { get; set; }
        public int SortOrder { get; set; }
    }
}
