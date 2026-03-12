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

        /// <summary>
        /// 检查同平台下是否存在同名软件（排除指定 ID 的记录）。
        /// </summary>
        /// <param name="name">软件名称。</param>
        /// <param name="platformId">平台 ID。</param>
        /// <param name="excludeId">需要排除的软件 ID（用于编辑时排除自身）。</param>
        /// <returns>是否已存在。</returns>
        Task<bool> CheckNameExistsAsync(string name, string platformId, string? excludeId);
    }
}
