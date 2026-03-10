using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Interfaces
{
    public interface IPlatformService
    {
        Task<List<PlatformResponse>> GetAllAsync();
        Task<PlatformResponse> CreateAsync(PlatformCreateRequest request);
        Task<PlatformResponse?> UpdateAsync(string id, PlatformUpdateRequest request);
        Task<bool> DeleteAsync(string id);
    }
}
