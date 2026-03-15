using DownloadStation.Server.Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// 扫描未绑定的物理文件。
        /// </summary>
        /// <param name="customPath">可选的自定义扫描路径。</param>
        /// <returns>未绑定文件列表。</returns>
        Task<List<UnboundFileResponse>> ScanUnboundFilesAsync(string? customPath = null);
    }
}
