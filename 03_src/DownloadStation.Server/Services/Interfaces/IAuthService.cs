using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Interfaces
{
    /// <summary>
    /// 系统核心验签和 JWT 派发基础接口。
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 基于全局固定密码验证请求方的入驻资格，验证通过后下发带有具体过期周期的 JWT 通行证。
        /// </summary>
        /// <param name="password">来自用户键入的口令字符串。</param>
        /// <returns>如校验成功则返回有效的 JWT；如不成功则返回空字符串或 null。</returns>
        Task<string?> LoginAsync(string password);
    }
}
