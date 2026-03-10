using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Public
{
    [ApiController]
    [Route("api/platforms")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService _platformService;

        public PlatformsController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _platformService.GetAllAsync();
            return Ok(Dtos.Responses.ApiResponse<object>.Success(data));
        }
    }
}
