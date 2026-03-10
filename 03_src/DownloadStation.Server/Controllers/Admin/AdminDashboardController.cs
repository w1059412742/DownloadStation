using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public AdminDashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var data = await _dashboardService.GetStatisticsAsync();
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }
    }
}
