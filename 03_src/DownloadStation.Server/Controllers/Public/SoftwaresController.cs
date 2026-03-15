using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Public
{
    [ApiController]
    [Route("api/softwares")]
    public class SoftwaresController : ControllerBase
    {
        private readonly ISoftwareService _softwareService;

        public SoftwaresController(ISoftwareService softwareService)
        {
            _softwareService = softwareService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? categoryId, [FromQuery] string? platformId,
            [FromQuery] string? keyword, [FromQuery] string? sortBy,
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            // 前台不获取草稿，includeDrafts 为 false
            var data = await _softwareService.GetPagedListAsync(categoryId, platformId, null, keyword, sortBy, false, page, pageSize);
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await _softwareService.GetByIdAsync(id);
            if (data == null || data.Status == Models.Enums.SoftwareStatus.Draft)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "软件查询不到或者已下架。"));

            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }
    }
}
