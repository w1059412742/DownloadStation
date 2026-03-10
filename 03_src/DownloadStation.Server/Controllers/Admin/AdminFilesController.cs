using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Admin
{
    /// <summary>
    /// 提供涉及 NAS 本地直通读写的各种超级管理特性端口。
    /// </summary>
    [ApiController]
    [Route("api/admin/files")]
    [Authorize]
    public class AdminFilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IVersionService _versionService;

        public AdminFilesController(IFileService fileService, IVersionService versionService)
        {
            _fileService = fileService;
            _versionService = versionService;
        }

        [HttpGet("scan")]
        public async Task<IActionResult> ScanUnbound()
        {
            var data = await _fileService.ScanUnboundFilesAsync();
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }

        [HttpPost("bind")]
        public async Task<IActionResult> BindFile([FromBody] FileBindRequest request)
        {
            try
            {
                var req = new VersionCreateRequest
                {
                    SoftwareId = request.SoftwareId,
                    VersionNumber = request.VersionNumber,
                    Changelog = request.Changelog,
                    FilePath = request.FilePath
                };

                var versionObj = await _versionService.CreateAsync(req);
                return Ok(Dtos.Responses.ApiResponse<object>.Success(versionObj, "文件入库并挂载版本记录成功。防篡改检验列队工作中..."));
            }
            catch (Exception ex)
            {
                return BadRequest(Dtos.Responses.ApiResponse<object>.Fail(400, "文件绑定失效：" + ex.Message));
            }
        }
    }
}
