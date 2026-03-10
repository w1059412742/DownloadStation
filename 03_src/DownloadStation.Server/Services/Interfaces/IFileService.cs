using DownloadStation.Server.Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Interfaces
{
    public interface IFileService
    {
        Task<List<UnboundFileResponse>> ScanUnboundFilesAsync();
    }
}
