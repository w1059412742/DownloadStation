using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/versions")]
    [Authorize]
    public class AdminVersionsController : ControllerBase
    {
        private readonly IVersionService _versionService;

        public AdminVersionsController(IVersionService versionService)
        {
            _versionService = versionService;
        }

        [HttpGet("software/{softwareId}")]
        public async Task<IActionResult> GetBySoftwareId(string softwareId)
        {
            var data = await _versionService.GetBySoftwareIdAsync(softwareId, includeHidden: true);
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }

        [HttpPost("upload")]
        [RequestSizeLimit(1073741824)] // 限制 1GB
        public async Task<IActionResult> Upload(
            [FromForm] string softwareId, 
            [FromForm] string versionNumber, 
            [FromForm] string? changelog, 
            Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(Dtos.Responses.ApiResponse<object>.Fail(400, "未选择文件。"));

            try
            {
                var data = await _versionService.UploadAsync(softwareId, versionNumber, changelog, file);
                return Ok(Dtos.Responses.ApiResponse<object>.Success(data, "安装包上传并绑定成功。"));
            }
            catch (System.Exception ex)
            {
                return BadRequest(Dtos.Responses.ApiResponse<object>.Fail(400, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] VersionUpdateRequest request)
        {
            var data = await _versionService.UpdateAsync(id, request);
            if (data == null)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "版本记录不存在。"));

            return Ok(Dtos.Responses.ApiResponse<object>.Success(data, "版本信息修改成功。"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, [FromQuery] bool physicalDelete = false)
        {
            var success = await _versionService.DeleteAsync(id, physicalDelete);
            if (!success)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "版本记录不存在。"));

            return Ok(Dtos.Responses.ApiResponse<object?>.Success(null, physicalDelete ? "版本及物理文件已双清。" : "版本记录删除完毕。"));
        }

        [HttpPatch("{id}/visibility")]
        public async Task<IActionResult> ChangeVisibility(string id, [FromBody] int isVisible)
        {
            var success = await _versionService.ChangeVisibilityAsync(id, isVisible);
            if (!success)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "版本记录不存在。"));

            return Ok(Dtos.Responses.ApiResponse<object?>.Success(null, "可见性切换成功。"));
        }
    }
}
