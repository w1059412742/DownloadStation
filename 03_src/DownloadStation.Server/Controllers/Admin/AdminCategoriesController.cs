using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DownloadStation.Server.Data;
using Microsoft.EntityFrameworkCore;
using DownloadStation.Server.Models;
using DownloadStation.Server.Models.Enums;
using DownloadStation.Server.Dtos.Responses;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/categories")]
    [Authorize] // 拦截，要求拥有合法的 JWT
    public class AdminCategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public AdminCategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTree()
        {
            var data = await _categoryService.GetTreeAsync();
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            var data = await _categoryService.CreateAsync(request);
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data, "分类创建成功。"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CategoryUpdateRequest request)
        {
            var data = await _categoryService.UpdateAsync(id, request);
            if (data == null)
                return NotFound(Dtos.Responses.ApiResponse<object>.Fail(404, "目标分类不存在。"));

            return Ok(Dtos.Responses.ApiResponse<object>.Success(data, "分类修改成功。"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteAsync(id);
            return Ok(ApiResponse<bool>.Success(true));
        }

        [HttpPost("seed")]
        [AllowAnonymous]
        public async Task<IActionResult> SeedBaseData([FromServices] AppDbContext context)
        {
            if (await context.Categories.AnyAsync()) return Ok(ApiResponse<bool>.Success(true));

            var cat1 = new Category { Id = Guid.NewGuid().ToString("N"), Name = "开发工具", SortOrder = 1 };
            var cat2 = new Category { Id = Guid.NewGuid().ToString("N"), Name = "生产力工具", SortOrder = 2 };
            var cat3 = new Category { Id = Guid.NewGuid().ToString("N"), Name = "系统镜像", SortOrder = 3 };

            context.Categories.AddRange(cat1, cat2, cat3);

            var p1 = new Platform { Id = Guid.NewGuid().ToString("N"), Name = "Windows", ColorHex = "#0078D6" };
            var p2 = new Platform { Id = Guid.NewGuid().ToString("N"), Name = "macOS", ColorHex = "#333333" };
            var p3 = new Platform { Id = Guid.NewGuid().ToString("N"), Name = "Linux", ColorHex = "#FCC624" };

            context.Platforms.AddRange(p1, p2, p3);

            var sw = new Software
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = "JetBrains Rider",
                Summary = "跨平台 .NET IDE",
                Description = "基于 IntelliJ 平台的跨平台 .NET IDE，支持 C#, F#, ASP.NET Core 等。",
                CategoryId = cat1.Id,
                PlatformId = p1.Id, // 设置单一平台
                Status = SoftwareStatus.Published,
                TotalDownloads = 523
            };

            context.Softwares.Add(sw);

            await context.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Success(true));
        }
    }
}
