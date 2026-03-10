using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/platforms")]
    [Authorize]
    public class AdminPlatformsController : ControllerBase
    {
        private readonly IPlatformService _platformService;

        public AdminPlatformsController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _platformService.GetAllAsync();
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlatformCreateRequest request)
        {
            var data = await _platformService.CreateAsync(request);
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data, "平台创建成功。"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] PlatformUpdateRequest request)
        {
            var data = await _platformService.UpdateAsync(id, request);
            if (data == null)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "目标平台不存在。"));

            return Ok(Dtos.Responses.ApiResponse<object>.Success(data, "平台资料修改成功。"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _platformService.DeleteAsync(id);
            if (!success)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "目标平台不存在。"));

            return Ok(Dtos.Responses.ApiResponse<object?>.Success(null, "平台资料删除成功。"));
        }
    }
}
