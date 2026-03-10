using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Interfaces
{
    public interface IVersionService
    {
        Task<List<VersionResponse>> GetBySoftwareIdAsync(string softwareId, bool includeHidden);
        
        Task<VersionResponse?> GetByIdAsync(string id);

        Task<VersionResponse> CreateAsync(VersionCreateRequest request);
        
        Task<VersionResponse> UploadAsync(string softwareId, string versionNumber, string? changelog, Microsoft.AspNetCore.Http.IFormFile file);

        Task<VersionResponse?> UpdateAsync(string id, VersionUpdateRequest request);

        Task<bool> DeleteAsync(string id, bool physicalDelete);

        Task<bool> ChangeVisibilityAsync(string id, int isVisible);

        Task IncrementDownloadAsync(string id);
    }
}
