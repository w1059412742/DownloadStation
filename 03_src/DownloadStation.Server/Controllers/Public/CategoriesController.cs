using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Public
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetTree()
        {
            var data = await _categoryService.GetTreeAsync();
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }
    }
}
