using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Dtos.Responses;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Admin
{
    /// <summary>
    /// 后台管理用的标签管理控制器。
    /// </summary>
    [ApiController]
    [Route("api/admin/tags")]
    [Authorize]
    public class AdminTagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public AdminTagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// 获取所有定义的软件标签。
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _tagService.GetAllAsync();
            return Ok(ApiResponse<List<TagResponse>>.Success(data));
        }

        /// <summary>
        /// 创建新标签。
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagCreateRequest request)
        {
            var data = await _tagService.CreateAsync(request);
            return Ok(ApiResponse<TagResponse>.Success(data, "标签创建成功。"));
        }

        /// <summary>
        /// 更新标签资料。
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TagUpdateRequest request)
        {
            var data = await _tagService.UpdateAsync(id, request);
            if (data == null)
                return NotFound(ApiResponse<object>.Fail(404, "目标标签不存在。"));

            return Ok(ApiResponse<TagResponse>.Success(data, "标签修改成功。"));
        }

        /// <summary>
        /// 物理删除标签记录。
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _tagService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.Fail(404, "目标标签不存在。"));

            return Ok(ApiResponse<object?>.Success(null, "标签删除成功。"));
        }
    }
}
