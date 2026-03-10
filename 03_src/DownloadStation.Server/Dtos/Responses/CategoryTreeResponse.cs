using System.Collections.Generic;

namespace DownloadStation.Server.Dtos.Responses
{
    public class CategoryTreeResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ParentId { get; set; }
        public int SortOrder { get; set; }
        public List<CategoryTreeResponse> Children { get; set; } = new();
    }
}
