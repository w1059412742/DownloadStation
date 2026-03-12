using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/softwares")]
    [Authorize]
    public class AdminSoftwaresController : ControllerBase
    {
        private readonly ISoftwareService _softwareService;

        public AdminSoftwaresController(ISoftwareService softwareService)
        {
            _softwareService = softwareService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? categoryId, [FromQuery] string? platformId,
            [FromQuery] string? keyword, [FromQuery] string? sortBy,
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            // 后台获取全量（包括草稿）
            var data = await _softwareService.GetPagedListAsync(categoryId, platformId, keyword, sortBy, true, page, pageSize);
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await _softwareService.GetByIdAsync(id);
            if (data == null)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "软件查询不到。"));

            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SoftwareCreateRequest request)
        {
            var data = await _softwareService.CreateAsync(request);
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data, "软件创建成功。"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] SoftwareUpdateRequest request)
        {
            var data = await _softwareService.UpdateAsync(id, request);
            if (data == null)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "目标软件不存在。"));

            return Ok(Dtos.Responses.ApiResponse<object>.Success(data, "软件修改成功。"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _softwareService.DeleteAsync(id);
            if (!success)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "目标软件不存在。"));

            return Ok(Dtos.Responses.ApiResponse<object?>.Success(null, "软件删除成功。"));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(string id, [FromBody] int status)
        {
            var success = await _softwareService.ChangeStatusAsync(id, status);
            if (!success)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "目标软件不存在。"));

            return Ok(Dtos.Responses.ApiResponse<object?>.Success(null, "软件状态更新成功。"));
        }

        /// <summary>
        /// 检查同平台下是否存在同名软件，用于保存前的前端校验。
        /// </summary>
        [HttpGet("check-name")]
        public async Task<IActionResult> CheckName(
            [FromQuery] string name, [FromQuery] string platformId, [FromQuery] string? excludeId)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(platformId))
                return Ok(Dtos.Responses.ApiResponse<object>.Success(new { exists = false }));

            var exists = await _softwareService.CheckNameExistsAsync(name, platformId, excludeId);
            return Ok(Dtos.Responses.ApiResponse<object>.Success(new { exists }));
        }

        [HttpPost("upload-image")]
        [RequestSizeLimit(10485760)] // 10MB
        public async Task<IActionResult> UploadImage(
            [FromServices] Microsoft.Extensions.Configuration.IConfiguration config,
            Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(Dtos.Responses.ApiResponse<object>.Fail(400, "未选择文件。"));

            try
            {
                var basePath = config.GetValue<string>("AppSettings:UploadBasePath") ?? "./uploads";
                // 按照日期分目录
                var dateFolder = System.DateTime.Now.ToString("yyyyMMdd");
                var folderPath = System.IO.Path.Combine(basePath, "images", dateFolder);

                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }

                var ext = System.IO.Path.GetExtension(file.FileName);
                var newFileName = $"{System.Guid.NewGuid():N}{ext}";
                var filePath = System.IO.Path.Combine(folderPath, newFileName);

                using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 返回相对的 URL 访问路径
                var urlPath = $"/uploads/images/{dateFolder}/{newFileName}";
                return Ok(Dtos.Responses.ApiResponse<object>.Success(new { url = urlPath }, "图片上传成功。"));
            }
            catch (System.Exception ex)
            {
                return BadRequest(Dtos.Responses.ApiResponse<object>.Fail(400, "图片上传失败：" + ex.Message));
            }
        }
    }
}
