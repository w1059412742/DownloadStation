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
    }
}
