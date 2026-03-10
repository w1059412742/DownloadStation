using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Dtos.Responses;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DownloadStation.Server.Controllers.Admin
{
    /// <summary>
    /// 后台认证鉴权中枢。
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// 基于全局密码的主站签发接口。
        /// </summary>
        /// <param name="request">包含前端表单提交密码的基础传输承载体。</param>
        /// <returns>返回有效签名的基于角色的 Token 字段流。</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Ok(ApiResponse<string>.Fail(401, "密码错误，拒绝授权接入。"));
            }

            // 出于 REST 标准化的抽象，直接将单一字符串返回给前端存储调用
            return Ok(ApiResponse<object>.Success(new { token }, "登录成功，欢迎掌控局势！"));
        }
    }
}
