using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Public
{
    [ApiController]
    [Route("api/softwares")]
    public class DownloadController : ControllerBase
    {
        private readonly IVersionService _versionService;

        public DownloadController(IVersionService versionService)
        {
            _versionService = versionService;
        }

        [HttpGet("{softwareId}/versions/{versionId}/download")]
        public async Task<IActionResult> DownloadFile(string softwareId, string versionId)
        {
            var version = await _versionService.GetByIdAsync(versionId);

            if (version == null || version.SoftwareId != softwareId || version.IsVisible == 0)
                return NotFound("请求的版本对象不存在或已被下架。");

            if (!System.IO.File.Exists(version.FilePath))
                return NotFound("底层物理文件丢失，请联系站长进行修复。");

            // 统计 + 1（异步进行不阻塞下载管道准备）
            _ = _versionService.IncrementDownloadAsync(versionId);

            // 自动推断通用 octet-stream 保证浏览器弹出下载页
            var contentType = "application/octet-stream";
            return PhysicalFile(Path.GetFullPath(version.FilePath), contentType, version.FileName, enableRangeProcessing: true);
        }

        [HttpGet("{softwareId}/versions")]
        public async Task<IActionResult> GetVersions(string softwareId)
        {
            // 前台公共调用只吐出可见的实体日志
            var data = await _versionService.GetBySoftwareIdAsync(softwareId, includeHidden: false);
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }
    }
}
