namespace DownloadStation.Server.Dtos.Requests
{
    /// <summary>
    /// 管理员统一网关登录请求封装。
    /// </summary>
    public class LoginRequest
    {
        public string Password { get; set; } = string.Empty;
    }
}
