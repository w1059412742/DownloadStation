namespace DownloadStation.Server.Dtos.Requests
{
    public class CategoryUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? ParentId { get; set; }
        public int SortOrder { get; set; }
    }
}
