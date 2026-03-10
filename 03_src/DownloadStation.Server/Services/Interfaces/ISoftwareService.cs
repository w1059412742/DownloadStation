using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Dtos.Responses;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Interfaces
{
    public interface ISoftwareService
    {
        Task<PagedResult<SoftwareListResponse>> GetPagedListAsync(
            string? categoryId, string? platformId, string? keyword, string? sortBy, bool includeDrafts, int page, int pageSize);

        Task<SoftwareDetailResponse?> GetByIdAsync(string id);

        Task<SoftwareDetailResponse> CreateAsync(SoftwareCreateRequest request);

        Task<SoftwareDetailResponse?> UpdateAsync(string id, SoftwareUpdateRequest request);

        Task<bool> DeleteAsync(string id);

        Task<bool> ChangeStatusAsync(string id, int status);
    }
}
