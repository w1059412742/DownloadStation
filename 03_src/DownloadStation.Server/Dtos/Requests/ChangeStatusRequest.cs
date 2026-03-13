namespace DownloadStation.Server.Dtos.Requests
{
    /// <summary>
    /// 切换软件发布状态的请求 DTO。
    /// </summary>
    public class ChangeStatusRequest
    {
        /// <summary>
        /// 目标状态值：0 = 下架（Draft），1 = 发布（Published）。
        /// </summary>
        public int Status { get; set; }
    }
}
